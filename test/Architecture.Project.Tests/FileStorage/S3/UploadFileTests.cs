using Amazon.S3;
using Microsoft.Extensions.Logging;
using Moq;
using Architecture.Project.FileStorage.S3;

namespace Architecture.Project.Tests.FileStorage.S3;

public class UploadFileTests
{
    private readonly UploadFile _uploadFile;
    
    private readonly Mock<IAmazonS3> _client = new(); 
    private readonly Mock<ILogger<IUploadFile>> _logger = new();

    public UploadFileTests()
    {
        _uploadFile = new(_client.Object, _logger.Object);
    }

    [Fact]
    public async Task Handle_FolderIsEmpty_SetFileNameWithoutTheFolder()
    {
        await _uploadFile.Handle("aaa", "", "ccc", "ddd", CancellationToken.None);

        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, type) => @object.ToString()!.Contains("Uploading file to the root") && type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }
    
    
    [Fact]
    public async Task Handle_FolderIsNotEmpty_SetFileNameWithTheFolder()
    {
        await _uploadFile.Handle("aaa", "bbb/", "ccc", "ddd", CancellationToken.None);

        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, type) => @object.ToString()!.Contains("Uploading file to the root") && type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Never);
        
        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, type) => @object.ToString()!.Contains("Uploading to folder bbb") && type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
        
        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, type) => @object.ToString()!.Contains("Adding / to the end of folder name") && type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Never);
    }
    
    
    [Fact]
    public async Task Handle_FolderDontHaveSlash_AddSlashToTheFolder()
    {
        await _uploadFile.Handle("aaa", "bbb", "ccc", "ddd", CancellationToken.None);

        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, type) => @object.ToString()!.Contains("Adding / to the end of folder name") && type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }
}