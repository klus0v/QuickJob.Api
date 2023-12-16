using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using QuickJob.DataModel.Api.Base;
using QuickJob.DataModel.Configuration;
using Vostok.Configuration.Abstractions;
using Vostok.Logging.Abstractions;

namespace QuickJob.BusinessLogic.Storages.S3;

public class AWSStorage : IS3Storage
{
    private readonly ILog log;
    private readonly AmazonS3Client s3Client;
    private readonly S3Settings s3Settings;

    public AWSStorage(ILog log, AmazonS3Client s3Client, IConfigurationProvider configurationProvider)
    {
        this.log = log;
        this.s3Client = s3Client;
        s3Settings = configurationProvider.Get<S3Settings>();
    }

    public async Task<EntityResult<List<string>>> UploadFiles(List<IFormFile> files)
    {
        var uploadTasks = files.Select(UploadFileInternal).ToArray();
        try
        {
            var tasksResults = await Task.WhenAll(uploadTasks);
            
            log.Info($"Successfully uploaded {files.Count} files");
            return EntityResult<List<string>>.CreateSuccessful(tasksResults.ToList());
        }
        catch (Exception e)
        {
            log.Error($"Error uploaded files; StackTrace: '{e.StackTrace}'.");
            return EntityResult<List<string>>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }

    public async Task<EntityResult<string>> UploadFile(IFormFile file)
    {
        try
        {
            var fileUrl= await UploadFileInternal(file);
            
            log.Info($"Successfully uploaded file: '{file.FileName}'");
            return EntityResult<string>.CreateSuccessful(fileUrl);
        }
        catch (Exception e)
        {
            log.Error($"Error uploaded file; StackTrace: '{e.StackTrace}'.");
            return EntityResult<string>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }

    private async Task<string> UploadFileInternal(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var request = new PutObjectRequest
        {
            BucketName = s3Settings.BucketName,
            Key = $"{s3Settings.RootPath}/{fileName}",
            InputStream = file.OpenReadStream(),
            ContentType = file.ContentType
        };
        
        await s3Client.PutObjectAsync(request);
        
        return $"{s3Settings.FilesBaseUrl}/{fileName}";
    }
}