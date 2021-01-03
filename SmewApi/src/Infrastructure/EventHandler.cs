using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.Hosting;
using Smew.Infrastructure;

namespace Smew.Handlers
{
    public abstract class EventHandler<T>: BackgroundService {
        protected Subject<RedisEvent<T>> sub = new Subject<RedisEvent<T>>();
        protected IObservable<RedisEvent<T>> obs => sub.SubscribeOn(Scheduler.Default).ObserveOn(Scheduler.Default);
    }
}
