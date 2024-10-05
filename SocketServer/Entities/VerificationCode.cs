using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocketServer.Entities;

public class VerificationCode
{
    [Key]
    public int IdCode { get; set; }
    public string Code { get; set; }
    public string Email { get; set; }
    public string AccessLocation { get; set; }
    [ForeignKey("enDeviceOS")]
    public EnDeviceOS EnDeviceOs { get; set; }
}