namespace SocketServer.Services;

public class FileTransferService
{
    private readonly string _tempDirectory = Path.Combine(Path.GetTempPath(), "FileTransferTemp");
    
    public FileTransferService()
    {
        if (!Directory.Exists(_tempDirectory))
        {
            Directory.CreateDirectory(_tempDirectory);
        }
    }
    
    public async Task ReceiveFile(string fileName, byte[] chunk)
    {
        var filePath = Path.Combine(_tempDirectory, fileName);
        using (var fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
        {
            await fileStream.WriteAsync(chunk, 0, chunk.Length);
        }
    }
    
    public async Task SaveCompleteFile(string fileName, string targetDirectory)
    {
        var tempFilePath = Path.Combine(_tempDirectory, fileName);
        var targetFilePath = Path.Combine(targetDirectory, fileName);
        
        if (File.Exists(tempFilePath))
        {
            File.Move(tempFilePath, targetFilePath);
        }
    }

    public async Task DeleteTempFile(string fileName)
    {
        var filePath = Path.Combine(_tempDirectory, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public async Task<byte[]> GetFile(string fileName)
    {
        var filePath = Path.Combine(_tempDirectory, fileName);
        if (File.Exists(filePath))
        {
            return await File.ReadAllBytesAsync(filePath);
        }

        return null;
    }
}