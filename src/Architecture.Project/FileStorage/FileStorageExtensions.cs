using Amazon.S3;
using Architecture.Project.FileStorage.S3;
using Microsoft.Extensions.DependencyInjection;

namespace Architecture.Project.FileStorage;

public static class FileStorageExtensions
{
    public static void AddServicesFromFileStorage(this IServiceCollection services, bool isProduction, string serviceUrl)
    {
        if (isProduction)
            services.AddTransient<IAmazonS3, AmazonS3Client>();
        else
            services.AddTransient<IAmazonS3>(_ =>
                new AmazonS3Client(
                    new AmazonS3Config
                    {
                        ServiceURL = serviceUrl
                    }));

        services.AddTransient<IDownloadFile, DownloadFile>();
        services.AddTransient<IUploadFile, UploadFile>();
    }
}