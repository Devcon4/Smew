using Moq;
using StackExchange.Redis;

namespace Smew.Builders {
    public static partial class MockBuilder {
        public static Mock<IDatabase> MockRedisDatabase() {
            var mock = new Mock<IDatabase>();
            
            return mock;
        }
    }
}
