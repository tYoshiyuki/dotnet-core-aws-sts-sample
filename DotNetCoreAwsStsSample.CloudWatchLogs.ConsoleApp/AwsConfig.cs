namespace DotNetCoreAwsStsSample.CloudWatchLogs.ConsoleApp
{
    class AwsConfig
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string SerialNumber { get; set; }
        public string RoleArn { get; set; }
        public string RoleSessionName { get; set; }
        public string LogGroupName { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string Query { get; set; }
    }
}
