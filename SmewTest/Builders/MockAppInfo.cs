using Microsoft.Extensions.Options;
using Moq;
using Smew.Infrastructure;

namespace Smew.Builders {
    public static partial class MockBuilder {
        private static AppInfo defaultConfig = new AppInfo("test01");
        public static Mock<IOptions<AppInfo>> MockAppInfo(AppInfo config = null) {
            if(config is null) {
                config = defaultConfig;
            }

            var mock = new Mock<IOptions<AppInfo>>();
            mock.SetupGet(e => e.Value).Returns(defaultConfig);
            
            return mock;
        }
    }
}
