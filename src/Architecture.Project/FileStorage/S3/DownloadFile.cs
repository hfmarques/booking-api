using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace Architecture.Project.FileStorage.S3;

public interface IDownloadFile
{
    Task<string?> HandleAsString(string bucketName, string folder, string fileName, CancellationToken cancellationToken);
    Task<byte[]?> HandleAsByteArray(string bucketName, string folder, string fileName, CancellationToken cancellationToken);
}
public class DownloadFile(IAmazonS3 client, ILogger<IDownloadFile> logger) : IDownloadFile
{
    public async Task<string?> HandleAsString(string bucketName, string folder, string fileName, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Downloading file {filename}", fileName);
            
            using var response = await GetFile(bucketName, folder, fileName, cancellationToken);

            using var ms = new StreamReader(response.ResponseStream);

            var file = await ms.ReadToEndAsync(cancellationToken);
            
            logger.LogInformation("File downloaded");
            
            return file;
        }
        catch (Exception e)
        {
            if(e.Message.Contains("The specified key does not exist"))
            {
                logger.LogWarning("File not found in the bucket");
                return null;
            }

            logger.LogError("Error downloading file from the bucket {e}", e);
            throw;
        }
    }
    
    public async Task<byte[]?> HandleAsByteArray(string bucketName, string folder, string fileName, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Downloading file {filename}", fileName);
            
            using var response = await GetFile(bucketName, folder, fileName, cancellationToken);

            using var ms = new MemoryStream();
            await response.ResponseStream.CopyToAsync(ms, cancellationToken);
            var file =  ms.ToArray();
            
            logger.LogInformation("File downloaded");

            return file;
        }
        catch (Exception e)
        {
            if(e.Message.Contains("The specified key does not exist"))
            {
                logger.LogWarning("File not found in the bucket");
                return null;
            }

            logger.LogError("Error downloading file from the bucket {e}", e);
            throw;
        }
    }
    
    private async Task<GetObjectResponse> GetFile(string bucketName, string folder, string fileName, CancellationToken cancellationToken)
    {
        GetObjectResponse? response = null;
        try
        {
            GetObjectRequest request;
            if (string.IsNullOrEmpty(folder))
            {
                logger.LogInformation("Downloading file from the root.");
                request = new()
                {
                    BucketName = bucketName,
                    Key = fileName,
                };
            }
            else
            {
                logger.LogInformation("Downloading from from folder {folder}", folder);
                if (!folder.EndsWith('/'))
                {
                    logger.LogInformation("Adding / to the end of folder name");
                    folder += '/';
                }
                request = new()
                {
                    BucketName = bucketName,
                    Key = folder + fileName,
                };
            }

            response = await client.GetObjectAsync(request, cancellationToken);
            return response;
        }
        catch
        {
            response?.Dispose();
            throw;
        }
    }
}