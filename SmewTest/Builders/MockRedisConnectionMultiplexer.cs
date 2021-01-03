using Moq;
using StackExchange.Redis;

namespace Smew.Builders {
    public static partial class MockBuilder {
        public static Mock<IConnectionMultiplexer> MockMockRedisConnectionMultiplexer() {
            var mock = new Mock<IConnectionMultiplexer>();
            
            return mock;
        }
    }
}
