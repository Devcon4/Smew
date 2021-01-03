using Moq;
using Smew.Infrastructure;

namespace Smew.Builders {
    public static partial class MockBuilder {
        public static Mock<IRedisProvider> MockRedisProvider() {
            var mock = new Mock<IRedisProvider>();

            return mock;
        }
    }
}