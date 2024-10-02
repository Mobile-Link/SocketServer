using System.Text;
using SocketServer.Services;

namespace TestsSocketServer.UnitTests.Services;

[TestFixture]
public class FileTransferServiceTests
{
    private FileTransferService _fileTransferService;
    private string _tempDirectory;
    
    [SetUp]
    public void SetUp()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "FileTransferTemp");
        _fileTransferService = new FileTransferService();
    }
    
    [Test]
    public async Task ReceiveFilee()
    {
        const string fileName = "test.txt";
        var chunk1 = "Hello, "u8.ToArray();
        var chunk2 = "World!"u8.ToArray();
        
        await _fileTransferService.ReceiveFile(fileName, chunk1);
        await _fileTransferService.ReceiveFile(fileName, chunk2);
        
        var filePath = Path.Combine(_tempDirectory, fileName);
        Assert.That(File.Exists(filePath), Is.True);
        
        var fileContent = await File.ReadAllTextAsync(filePath);
        Assert.That(fileContent, Is.EqualTo("Hello, World!"));
    }
    
    [Test]
    public async Task SaveCompleteFile()
    {
        const string fileName = "test.txt";
        var targetDirectory = Path.Combine(_tempDirectory, "target");
        Directory.CreateDirectory(targetDirectory);
        await _fileTransferService.ReceiveFile(fileName, "Hello, World!"u8.ToArray());
        
        await _fileTransferService.SaveCompleteFile(fileName, targetDirectory);
        
        var tempFilePath = Path.Combine(_tempDirectory, fileName);
        var targetFilePath = Path.Combine(targetDirectory, fileName);
        Assert.Multiple(() =>
        {
            Assert.That(File.Exists(tempFilePath), Is.False);
            Assert.That(File.Exists(targetFilePath), Is.True);
        });
    }
    
    [Test]
    public async Task DeleteTempFile()
    {
        const string fileName = "test.txt";
        await _fileTransferService.ReceiveFile(fileName, "Hello, World!"u8.ToArray());
        
        await _fileTransferService.DeleteTempFile(fileName);
        
        var filePath = Path.Combine(_tempDirectory, fileName);
        Assert.That(File.Exists(filePath), Is.False);
    }
    
    [Test]
    public async Task GetFile()
    {
        const string fileName = "test.txt";
        await _fileTransferService.ReceiveFile(fileName, "Hello, World!"u8.ToArray());
        
        var fileContent = await _fileTransferService.GetFile(fileName);
        Assert.Multiple(() =>
        {
            Assert.That(fileContent, Is.Not.Null);
            Assert.That(Encoding.UTF8.GetString(fileContent), Is.EqualTo("Hello, World!"));
        });
    }
    
    [Test]
    public async Task GetFile_WhenFileDoesNotExist()
    {
        const string fileName = "test.txt";
        
        var fileContent = await _fileTransferService.GetFile(fileName);
        
        Assert.That(fileContent, Is.Null);
    }
    
    [Test]
    public async Task GetFile_WhenFileIsDeleted()
    {
        const string fileName = "test.txt";
        await _fileTransferService.ReceiveFile(fileName, "Hello, World!"u8.ToArray());
        await _fileTransferService.DeleteTempFile(fileName);
        
        var fileContent = await _fileTransferService.GetFile(fileName);
        
        Assert.That(fileContent, Is.Null);
    }
    
    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, true);
        }
    }
}