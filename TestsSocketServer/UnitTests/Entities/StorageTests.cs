using SocketServer.Entities;

namespace TestsSocketServer.UnitTests.Entities;

[TestFixture]
public class StorageTests
{
    [Test]
    public void Storage_Properties_Valid()
    {
        var storage = new Storage
        {
            IdDevice = 1,
            User = new User(),
            StorageLimitBytes = 1024,
            UsedStorageBytes = 512
        };
        
        Assert.That(storage.IdDevice, Is.EqualTo(1));
        Assert.That(storage.User, Is.Not.Null);
        Assert.That(storage.StorageLimitBytes, Is.EqualTo(1024));
        Assert.That(storage.UsedStorageBytes, Is.EqualTo(512));
    }
}