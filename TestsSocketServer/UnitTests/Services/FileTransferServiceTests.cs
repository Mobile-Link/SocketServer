using System.Text;
using SocketServer.Services;

namespace TestsSocketServer.UnitTests.Services;

[TestFixture]
public class FileTransferServiceTests
{
    private FileTransferService _fileTransferService;
    private string _tempDirectory;
    private string _targetDirectory;
    
    [SetUp]
    public void SetUp()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "FileTransferTemp");
        Directory.CreateDirectory(_tempDirectory);

        _targetDirectory = Path.Combine(Path.GetTempPath(), "target");
        _fileTransferService = new FileTransferService();
    }
    
    [TestCase("test.txt")]
    [TestCase("test.jpg")]
    [TestCase("test.pdf")]
    public async Task ReceiveFile(string fileName)
    {
        var chunk1 = "Hello, "u8.ToArray();
        var chunk2 = "World!"u8.ToArray();
        
        await _fileTransferService.ReceiveFile(fileName, chunk1);
        await _fileTransferService.ReceiveFile(fileName, chunk2);
        
        var filePath = Path.Combine(_tempDirectory, fileName);
        Assert.That(File.Exists(filePath), Is.True);
        
        var fileContent = await File.ReadAllTextAsync(filePath);
        Assert.That(fileContent, Is.EqualTo("Hello, World!"));
    }
    
    [TestCase("test1.txt", "target1")]
    [TestCase("test2.txt", "target2")]
    public async Task SaveCompleteFile(string fileName, string targetDirectoryName)
    {
        var targetDirectory = Path.Combine(_targetDirectory, targetDirectoryName);
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
    
    [TestCase("test1.txt")]
    [TestCase("test2.txt")]
    public async Task DeleteTempFile(string fileName)
    {
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

        if (Directory.Exists(_targetDirectory))
        {
            Directory.Delete(_targetDirectory, true);
        }
    }
}
