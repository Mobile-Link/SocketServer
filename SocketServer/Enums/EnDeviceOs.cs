using System.ComponentModel;

namespace SocketServer.Enums;

public enum EnDeviceOs
{
    [Description("Linux")]
    Linux,
    
    [Description("Windows")]
    Windows,
    
    [Description("Android")]
    Android,
    
    [Description("IOS")]
    IOS,
    
    [Description("MacOS")]
    MacOS,
    
    [Description("Unknown")]
    Unknown,
}