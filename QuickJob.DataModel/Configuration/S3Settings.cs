namespace QuickJob.DataModel.Configuration;

public class S3Settings
{
    public string AccessKeyId { get; set; }
    public string SecretKey { get; set; }
    public string ServiceURL { get; set; }
    public string BucketName { get; set; }
    public string RootPath { get; set; }
    public string FilesBaseUrl { get; set; }
}