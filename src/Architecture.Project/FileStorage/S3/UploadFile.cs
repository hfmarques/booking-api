using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Logging;

namespace Architecture.Project.FileStorage.S3;

public interface IUploadFile
{
    Task Handle(string bucketName, string folder, string fileName, string file, CancellationToken cancellationToken);
    Task Handle(string bucketName, string folder, string fileName, byte[] file, CancellationToken cancellationToken);
}
public class UploadFile(IAmazonS3 client, ILogger<IUploadFile> logger) : IUploadFile
{
    public async Task Handle(string bucketName, string folder, string fileName, string file, CancellationToken cancellationToken)
    {
        logger.LogInformation("Uploading file {filename}", fileName);
        
        await using var fileToUpload = new MemoryStream();
        var writer = new StreamWriter(fileToUpload);
        await writer.WriteAsync(file);
        await writer.FlushAsync(cancellationToken);
        fileToUpload.Position = 0;
        
        await Upload(bucketName, folder, fileName, fileToUpload, cancellationToken);
        
        logger.LogInformation("File uploaded");
    }
    
    public async Task Handle(string bucketName, string folder, string fileName, byte[] file, CancellationToken cancellationToken)
    {
        logger.LogInformation("Uploading file {filename}", fileName);

        await using var fileToUpload = new MemoryStream(file);

        await Upload(bucketName, folder, fileName, fileToUpload, cancellationToken);
        
        logger.LogInformation("File uploaded");
    }

    private async Task Upload(string bucketName, string folder, string fileName, Stream file, CancellationToken cancellationToken)
    {
        var fileTransferUtility = new TransferUtility(client);
        
        if (string.IsNullOrWhiteSpace(folder))
        {
            logger.LogInformation("Uploading file to the root.");
            await fileTransferUtility.UploadAsync(file, bucketName, fileName, cancellationToken);
            return;
        }

        logger.LogInformation("Uploading to folder {folder}", folder);
        if (!folder.EndsWith('/'))
        {
            logger.LogInformation("Adding / to the end of folder name");
            folder += '/';
        }
        
        await fileTransferUtility.UploadAsync(file, bucketName, folder + fileName, cancellationToken);
    }
}