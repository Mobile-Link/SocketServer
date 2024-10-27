using System.ComponentModel;

namespace SocketServer.Enums;

public enum EnChunkStatus
{
    [Description("Pending")]
    Pending = 1,
    
    [Description("Received")]
    Received = 2,
    
    [Description("Error")]
    Error = 3
}