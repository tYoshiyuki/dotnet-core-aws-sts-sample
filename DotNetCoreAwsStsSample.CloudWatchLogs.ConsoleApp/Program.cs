using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using DotNetCoreAwsStsSample.Library;
using Microsoft.Extensions.Configuration;

namespace DotNetCoreAwsStsSample.CloudWatchLogs.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Enter MFA code: ");
            var mfaCode = Console.ReadLine();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var awsConfig = new AwsConfig();
            configuration.GetSection(nameof(AwsConfig)).Bind(awsConfig);

            var credentials = await AwsStsUtil.GetCredentialsAsync(awsConfig.AccessKey, awsConfig.SecretKey, mfaCode, awsConfig.SerialNumber, awsConfig.RoleArn, awsConfig.RoleSessionName);

            var from = DateTime.ParseExact(awsConfig.From.ToString(), "yyyyMMdd", null);
            var to = DateTime.ParseExact(awsConfig.To.ToString(), "yyyyMMdd", null);

            // CloudWatch Logs Insight のクエリを実行します
            IAmazonCloudWatchLogs client = new AmazonCloudWatchLogsClient(credentials, RegionEndpoint.APNortheast1);
            var startQueryRequest = new StartQueryRequest
            {
                LogGroupName = awsConfig.LogGroupName,
                StartTime = new DateTimeOffset(from.ToUniversalTime()).ToUnixTimeSeconds(),
                EndTime = new DateTimeOffset(to.ToUniversalTime()).ToUnixTimeSeconds(),
                QueryString = awsConfig.Query,
                Limit = 1000
            };
            var startQueryResponse = await client.StartQueryAsync(startQueryRequest);

            var getQueryRequest = new GetQueryResultsRequest
            {
                QueryId = startQueryResponse.QueryId
            };

            // クエリ実行完了まで実行結果をポーリングします
            var getQueryResults = await client.GetQueryResultsAsync(getQueryRequest);
            while (getQueryResults.Status == QueryStatus.Running || getQueryResults.Status == QueryStatus.Scheduled)
            {
                await Task.Delay(3 * 1000);
                getQueryResults = await client.GetQueryResultsAsync(getQueryRequest);
            }

            // TODO 例外処理の実装方法見直し
            if (getQueryResults.Status != QueryStatus.Complete)
            {
                throw new Exception($"Query failed. QueryResult status is {getQueryResults.Status}.");
            }

            foreach (var result in getQueryResults.Results)
            {
                Console.WriteLine(result.Skip(1).First().Value);
            }
            Console.ReadKey();
        }
    }
}
