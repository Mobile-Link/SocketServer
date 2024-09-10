using System.Globalization;

namespace SocketServer.Models;

public class FileTransfer
{
    public string Name { get; set; }
    public long TotalSize { get; set; }
    public long Progress { get; set; }
}