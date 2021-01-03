using Xunit;
using System.Threading.Tasks;
using Smew.Infrastructure;
using Smew.Events;
using Smew.Builders;
using Moq;

namespace SmewTest.Unit
{
    public class RedisMessageBusTests {
        [Fact]
        public void CreateMessageBus() {
            var sut = new RedisMessageBus(MockBuilder.MockRedisProvider().Object, MockBuilder.MockAppInfo().Object);

            Assert.NotNull(sut);
        }

        [Fact]
        public async Task CanPublishMessage() {
            var mockRedisProvider = MockBuilder.MockRedisProvider();
            var mockRedis = MockBuilder.MockMockRedisConnectionMultiplexer();
            var mockRedisDatabase = MockBuilder.MockRedisDatabase();
            mockRedis.Setup(s => s.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(mockRedisDatabase.Object);
            mockRedisProvider.SetupProperty(s => s.redis, mockRedis.Object);
            var sut = new RedisMessageBus(mockRedisProvider.Object, MockBuilder.MockAppInfo().Object);

            await sut.PublishAsync(new NoOpEvent());

            mockRedis.Verify(s => s.GetDatabase(It.IsAny<int>(), It.IsAny<object>()), Times.AtLeastOnce);
        }
    }
}