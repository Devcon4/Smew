using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Smew.Events;
using Smew.Infrastructure;

namespace Smew.Handlers {

    public class PostMessageHandler : EventHandler<PostMessageEvent> {
        private readonly ILogger<PostMessageHandler> _logger;

        public IMessageBus _messageBus { get; }

        public PostMessageHandler (ILogger<PostMessageHandler> logger, IMessageBus messageBus) {
            _logger = logger;
            _messageBus = messageBus;
        }

        protected override async Task ExecuteAsync (CancellationToken stoppingToken) {
            obs.Subscribe (e => { });
            await _messageBus.SubscribeAsync<PostMessageEvent> (sub);
        }
    }
}