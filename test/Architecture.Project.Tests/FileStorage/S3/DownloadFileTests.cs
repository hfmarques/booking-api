using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Moq;
using Architecture.Project.FileStorage.S3;

namespace Architecture.Project.Tests.FileStorage.S3;

public class DownloadFileTests
{
    private readonly DownloadFile _downloadFile;
    private readonly Mock<IAmazonS3> _client = new();
    private readonly Mock<ILogger<IDownloadFile>> _logger = new();
    public DownloadFileTests()
    {
        _downloadFile = new(_client.Object, _logger.Object);

        var fileToUpload = new MemoryStream();
        var writer = new StreamWriter(fileToUpload);
        writer.Write("file");
        writer.Flush();
        fileToUpload.Position = 0;
        
        _client.Setup(x =>
                x.GetObjectAsync(It.IsAny<GetObjectRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse
            {
                ResponseStream = fileToUpload
            });
    }
    
    [Fact]
    public async Task Handle_FolderIsEmpty_SetKeyAFileName()
    {
        await _downloadFile.HandleAsString("aaa", "", "ccc", CancellationToken.None);

        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, type) => @object.ToString()!.Contains("Downloading file from the root") && type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Fact]
    public async Task Handle_FolderIsNotEmpty_SetKeyAsKeyPlusFolder()
    {
        await _downloadFile.HandleAsString("aaa", "bbb/", "ccc", CancellationToken.None);

        _client.Verify(x => x.GetObjectAsync(
            It.Is<GetObjectRequest>(y => y.Key.Contains("bbb/")),
            It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_FolderIsNotEmptyAndDontHaveSlash_SetKeyAsKeyPlusFolder()
    {
        await _downloadFile.HandleAsString("aaa", "bbb", "ccc", CancellationToken.None);

        _client.Verify(x => x.GetObjectAsync(
            It.Is<GetObjectRequest>(y => y.Key.Contains("bbb/")),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_FileNotFound_ThrowExpectedExceptionAndLogError()
    {
        _client.Setup(x =>
                x.GetObjectAsync(It.IsAny<GetObjectRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new("The specified key does not exist"));
        
        await _downloadFile.HandleAsString("aaa", "", "ccc", CancellationToken.None);

        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, type) => @object.ToString()!.Contains("File not found in the bucket") && type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }
    
    [Fact]
    public async Task Handle_OtherException_LogError()
    {
        _client.Setup(x =>
                x.GetObjectAsync(It.IsAny<GetObjectRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new());
        
        await Assert.ThrowsAsync<Exception>(() => _downloadFile.HandleAsString("aaa", "", "ccc", CancellationToken.None));

        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, type) => @object.ToString()!.Contains("Error downloading file from the bucket") && type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }
}