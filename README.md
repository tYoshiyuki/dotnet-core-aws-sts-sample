# dotnet-core-aws-sts-sample
.NET Core で AWS STS を実行し、一時クレデンシャルを取得するサンプル。

## Feature
- .NET5
- AWS SDK for .NET

## Project
- DotNetCoreAwsStsSample.Library
    - 共通ライブラリ
- DotNetCoreAwsStsSample.Sts.ConsoleApp
    - サンプル実装を行ったコンソールアプリケーション
- DotNetCoreAwsStsSample.CloudWatchLogs.ConsoleApp
    - CloudWatch Logs Insights のクエリを実行するコンソールアプリケーション

## Note
- AWS SDK for .NET を利用して STS にて、一時クレデンシャルを取得するサンプルです。
- appsettings.json の下記設定値を書き換えてご利用ください。

```json
{
  "AwsConfig": {
    "AccessKey": "xxxxxx",
    "SecretKey": "xxxxxx",
    "SerialNumber": "arn:aws:iam::xxxxxx:mfa/xxxxxx",
    "RoleArn": "arn:aws:iam::xxxxxx:role/xxxxxx",
    "RoleSessionName": "xxxxxx",
    "LogGroupName": "xxxxxx",
    "From": 20201201,
    "To": 20210101,
    "Query": "fields @timestamp, @message | sort @timestamp desc"
  }
}
```
