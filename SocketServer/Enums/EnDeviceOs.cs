using System.ComponentModel;

namespace SocketServer.Enums;

public enum EnDeviceOs
{
    [Description("Linux")]
    Linux = 1,
    
    [Description("Windows")]
    Windows = 2,
    
    [Description("Android")]
    Android = 3,
    
    [Description("IOS")]
    IOS = 4,
    
    [Description("MacOS")]
    MacOS = 5,
}