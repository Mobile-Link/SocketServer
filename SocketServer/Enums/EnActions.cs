using System.ComponentModel;

namespace SocketServer.Enums;

public enum EnActions
{
    [Description("Deleted device")]
    DeletedDevice,
    
    [Description("Added device")]
    AddedDevice,
    
    [Description("Changed password")]
    ChangedPassword,
    
    [Description("Changed device")]
    ChangedDevice,
    
    [Description("Changed user")]
    ChangedUser
}