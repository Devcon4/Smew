using Microsoft.Extensions.Logging;
using Moq;

namespace Smew.Builders {
    public static partial class MockBuilder {
        public static Mock<ILogger<T>> MockLogger<T>() {
            var mock = new Mock<ILogger<T>>();
            
            return mock;
        }
    }
}
