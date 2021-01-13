using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Smew.Infrastructure {

    public interface IMessageBus {
        Task<Task<T>> FirstAsync<T> (Guid trackingId);
        Task PublishAsync<T> (IReactiveEvent<T> Message, Guid trackingId = new Guid ());
        Task SubscribeAsync<T> (Subject<RedisEvent<T>> sub, string? consumerGroupId = null);
    }

    public class RedisEvent<T> {
        public string EventId { get; set; }
        public T EventBody { get; set; }
        public Guid TrackingId { get; set; }
    }

    public class RedisMessageBus : IMessageBus {
        private readonly IRedisProvider _redisProvider;
        private readonly AppInfo _appInfo;
        public readonly string _consumerGroupId = "SmewApi";
        private readonly string _eventDataId = "eventData";
        private readonly string _trackingId = "trackingId";
        private readonly string _eventPubChannelId = "eventPublished";
        public RedisMessageBus (IRedisProvider redisProvider, IOptions<AppInfo> appInfo) {
            _redisProvider = redisProvider;
            _appInfo = appInfo.Value;
        }
        public async Task PublishAsync<T> (IReactiveEvent<T> Message, Guid trackingId = new Guid ()) {
            var db = _redisProvider.redis.GetDatabase ();
            var typeName = Message.GetType ().Name;
            var eventString = JsonSerializer.Serialize (Message);
            var pairs = new NameValueEntry[] {
                new NameValueEntry (_eventDataId, eventString),
                new NameValueEntry (_trackingId, trackingId.ToString ()),
            };
            await db.StreamAddAsync (typeName, pairs);
            await db.PublishAsync (typeName, _eventPubChannelId);
        }

        public async Task<Task<T>> FirstAsync<T> (Guid trackingId) {
            var db = _redisProvider.redis.GetDatabase ();
            var typeName = typeof (T).Name;
            var sub = _redisProvider.redis.GetSubscriber ();
            var ret = new TaskCompletionSource<T> ();
            await sub.SubscribeAsync (typeName, async (ch, val) => {
                if (val == _eventPubChannelId) {
                    try {
                        await db.StreamCreateConsumerGroupAsync (typeName, _consumerGroupId);
                    } catch (System.Exception) { }

                    var evt = (await db.StreamReadGroupAsync (typeName, _consumerGroupId, _appInfo.InstanceId)).ToObservable ();
                    var match = await evt.Where (e => e.Values.Any (v => v.Name == _trackingId && v.Value == trackingId.ToString ())).FirstOrDefaultAsync ();
                    var data = match.Values.FirstOrDefault (v => v.Name == _eventDataId);
                    ret.TrySetResult (JsonSerializer.Deserialize<T> (data.Value));
                    await db.StreamAcknowledgeAsync (typeName, _consumerGroupId, match.Id);
                    await sub.UnsubscribeAsync (typeName);
                }
            });
            return ret.Task;
        }

        public async Task SubscribeAsync<T> (Subject<RedisEvent<T>> sub, string? consumerGroupId = null) {
            if (consumerGroupId is null) consumerGroupId = _consumerGroupId;

            var db = _redisProvider.redis.GetDatabase ();
            var eventType = typeof (T);
            var typeName = eventType.Name;

            var subscriber = _redisProvider.redis.GetSubscriber ();
            await subscriber.SubscribeAsync (typeName, async (ch, val) => {
                if (val == _eventPubChannelId) {
                    await getEntries ();
                }
                // await subscriber.UnsubscribeAsync (typeName);
            });

            async Task getEntries () {
                try {
                    await db.StreamCreateConsumerGroupAsync (typeName, consumerGroupId);
                } catch (System.Exception) { }

                var entries = await db.StreamReadGroupAsync (typeName, consumerGroupId, _appInfo.InstanceId, ">");
                foreach (var entry in entries) {
                    var val = entry.Values.FirstOrDefault (e => e.Name == _eventDataId);
                    var trackingId = entry.Values.FirstOrDefault (e => e.Name == _trackingId);
                    var obj = JsonSerializer.Deserialize<T> (val.Value);
                    sub.OnNext (new RedisEvent<T> { EventId = entry.Id, EventBody = obj, TrackingId = new Guid (trackingId.Value.ToString ()) });
                    await db.StreamAcknowledgeAsync (typeName, consumerGroupId, entry.Id);

                }

            }
        }
    }
}