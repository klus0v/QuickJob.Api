using Microsoft.AspNetCore.Http;
using QuickJob.DataModel.Api;

namespace QuickJob.BusinessLogic.Storages.S3;

public interface IS3Storage
{
    Task<EntityResult<string>> UploadFile(IFormFile file);
    Task<EntityResult<List<string>>> UploadFiles(List<IFormFile> file);
}