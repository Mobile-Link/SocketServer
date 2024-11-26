using System.ComponentModel;

namespace SocketServer.Enums;

public enum EnStatus
{
    [Description("Not started")]
    NotStarted,
    
    [Description("In progress")]
    InProgress,
    
    [Description("In cloud")]
    InCloud,
    
    [Description("Verifying")]
    Verifying,
    
    [Description("Finished")]
    Finished,
    
    [Description("Error")]
    Error,
    
    [Description("Canceled")]
    Canceled,
    
    [Description("Receiving Stalled")]
    ReceivingStalled,
}