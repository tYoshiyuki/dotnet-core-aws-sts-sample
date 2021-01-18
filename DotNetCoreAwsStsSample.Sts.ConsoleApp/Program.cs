using System;
using System.Threading.Tasks;
using DotNetCoreAwsStsSample.Library;
using Microsoft.Extensions.Configuration;

namespace DotNetCoreAwsStsSample.Sts.ConsoleApp
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
            Console.WriteLine("---------------------------------");
            Console.WriteLine($"AccessKey: {credentials.AccessKeyId}");
            Console.WriteLine($"SecretAccessKey: {credentials.SecretAccessKey}");
            Console.WriteLine($"SessionToken: {credentials.SessionToken}");
            Console.WriteLine("---------------------------------");
            Console.ReadKey();
        }
    }
}
