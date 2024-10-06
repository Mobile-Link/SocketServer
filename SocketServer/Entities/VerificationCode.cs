using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocketServer.Entities;

public class VerificationCode
{
    [BsonId]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    public string Code { get; set; }
    public string Email { get; set; }
    public DateTime InsertionDate { get; set; }
}