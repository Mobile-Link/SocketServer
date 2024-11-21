using System.ComponentModel;

namespace SocketServer.Enums;

public enum EnChunkStatus
{
    [Description("Pending")]
    Pending,
    
    [Description("Received")]
    Received,
    
    [Description("Error")]
    Error
}