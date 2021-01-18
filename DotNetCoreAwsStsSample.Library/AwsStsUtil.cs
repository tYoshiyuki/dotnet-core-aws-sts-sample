using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;

namespace DotNetCoreAwsStsSample.Library
{
    public static class AwsStsUtil
    {
        /// <summary>
        /// AssumeRoleを実行し一時クレデンシャルを取得します。
        /// </summary>
        /// <param name="accessKey">accessKey</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="mfaCode">mfaCode</param>
        /// <param name="serialNumber">serialNumber</param>
        /// <param name="roleArn">roleArn</param>
        /// <param name="roleSessionName">roleSessionName</param>
        /// <returns></returns>
        public static async Task<Credentials> GetCredentialsAsync(string accessKey, string secretKey, string mfaCode, string serialNumber, string roleArn, string roleSessionName)
        {
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var stsClient = new AmazonSecurityTokenServiceClient(credentials);

            var assumeRoleRequest = new AssumeRoleRequest
            {
                RoleArn = roleArn,
                RoleSessionName = roleSessionName,
                SerialNumber = serialNumber,
                TokenCode = mfaCode
            };

            var assumeRoleResponse = await stsClient.AssumeRoleAsync(assumeRoleRequest);
            return assumeRoleResponse.Credentials;
        }

        /// <summary>
        /// 一時クレデンシャルを取得します。
        /// </summary>
        /// <param name="accessKey">accessKey</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="mfaCode">mfaCode</param>
        /// <param name="serialNumber">serialNumber</param>
        /// <returns></returns>
        public static async Task<Credentials> GetCredentialsAsync(string accessKey, string secretKey, string mfaCode, string serialNumber)
        {
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var stsClient = new AmazonSecurityTokenServiceClient(credentials);

            var getSessionTokenRequest = new GetSessionTokenRequest
            {
                DurationSeconds = 3600,
                SerialNumber = serialNumber,
                TokenCode = mfaCode
            };

            var getSessionTokenResponse = await stsClient.GetSessionTokenAsync(getSessionTokenRequest);
            return getSessionTokenResponse.Credentials;
        }
    }
}
