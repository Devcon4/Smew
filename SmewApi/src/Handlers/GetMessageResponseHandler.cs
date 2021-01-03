using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Smew.Events;
using Smew.Infrastructure;

namespace Smew.Handlers
{
    public class GetMessageResponseHandler : EventHandler<GetMessageRequestEvent>
    {
        private readonly ILogger<GetMessageResponseHandler> _logger;
        private readonly IMessageBus _messageBus;

        public GetMessageResponseHandler(ILogger<GetMessageResponseHandler> logger, IMessageBus messageBus) {
            _logger = logger;
            _messageBus = messageBus;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            obs.Subscribe(e => {
                _logger.LogInformation("GetMessageRequestHandler: " + e.EventId);
                var body = new MessageEntity(e.EventBody.Body.Content, e.EventBody.Body.AuthorUid);
                _messageBus.PublishAsync(new GetMessageResponseEvent(body));
            });
            await _messageBus.SubscribeAsync<GetMessageRequestEvent>(sub);
        }
    }
}
