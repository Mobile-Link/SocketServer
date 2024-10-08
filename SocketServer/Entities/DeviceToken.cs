using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocketServer.Entities;

public class DeviceToken
{
    [BsonId]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string IdDeviceToken { get; set; }
    public long IdDevice { get; set; }
    public string Token { get; set; }
    public DateTime InsertionDate { get; set; }
}