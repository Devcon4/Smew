namespace Smew.Infrastructure
{
    public class AppInfo {
        public static string Kind = "AppInfo";
        public string InstanceId {get;set;}

        public AppInfo(string InstanceId) {
            this.InstanceId = InstanceId;
        }
        public AppInfo() {}
    };
}