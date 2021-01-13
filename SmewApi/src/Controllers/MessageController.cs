using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Smew.Events;
using Smew.Handlers;
using Smew.Infrastructure;

namespace SmewApi.Controllers {

    [ApiController]
    [Route ("[controller]")]
    public class MessageController {
        private readonly IMessageBus _messageBus;
        private readonly ILogger<MessageController> _logger;

        public MessageController (IMessageBus messageBus, ILogger<MessageController> logger) {
            _messageBus = messageBus;
            _logger = logger;
        }

        [HttpPost ()]
        public async Task PostMessageAsync (MessageMutation input) {
            var newInput = input with { Content = input.Content + " " + DateTime.Now.ToShortTimeString () };

            await _messageBus.PublishAsync (new PostMessageEvent (newInput));
        }

        [HttpGet ()]
        public async Task<MessageEntity> GetMessageAsync ([FromQuery] string AuthorUid) {
            var trackingId = Guid.NewGuid ();
            var ret = await _messageBus.FirstAsync<GetMessageResponseEvent> (trackingId);
            var input = new MessageQuery ("", AuthorUid);
            await _messageBus.PublishAsync (new GetMessageRequestEvent (input), trackingId);
            return (await ret).Body;
        }
    }

}