using Smew.Builders;
using Smew.Events;
using Smew.Infrastructure;
using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmewTest.Unit
{
    public class ReactiveEventBrokerTests {
        
        [Fact]
        public async System.Threading.Tasks.Task CanQueueEventAsync() {
            var mockMessageBus = MockBuilder.MockRedisMessageBus();
            // mockMessageBus.Setup(s => s.PublishAsync(It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var sut = new ReactiveEventBroker(mockMessageBus.Object);

            await sut.Queue(new NoOpEvent());
            // mockMessageBus.Verify(s => s.PublishAsync(It.IsAny<Type>(), It.IsAny<NoOpEvent>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }
    }
}