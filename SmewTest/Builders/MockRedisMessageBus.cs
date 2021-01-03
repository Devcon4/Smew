using Moq;
using Smew.Infrastructure;

namespace Smew.Builders {
    public static partial class MockBuilder {
        public static Mock<IMessageBus> MockRedisMessageBus() {
            var mock = new Mock<IMessageBus>();

            return mock;
        }
    }

}