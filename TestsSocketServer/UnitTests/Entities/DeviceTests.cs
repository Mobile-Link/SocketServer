using SocketServer.Entities;

namespace TestsSocketServer.UnitTests.Entities;

[TestFixture]
public class DeviceTests
{
    [Test]
    public void Device_Dates_Initialized()
    {
        var device = new Device
        {
            CreationDate = DateTime.Now,
            AlterationDate = DateTime.Now
        };

        Assert.Multiple(() =>
        {
            Assert.That(device.CreationDate, Is.Not.EqualTo(default(DateTime)));
            Assert.That(device.AlterationDate, Is.Not.EqualTo(default(DateTime)));
        });
    }

    [Test]
    public void Device_Properties_Valid()
    {
        var device = new Device
        {
            IdDevice = 1,
            User = new User(),
            IsDeleted = false,
            LastLocation = "LastLocation",
            AvailableSpace = 1024,
            OccupiedSpace = 512,
            Name = "Dispositivo de Teste",
            Token = "Token",
            CreationDate = DateTime.Now,
            AlterationDate = DateTime.Now
        };

        Assert.Multiple(() =>
        {
            Assert.That(device.IdDevice, Is.EqualTo(1));
            Assert.That(device.IsDeleted, Is.EqualTo(false));
            Assert.That(device.LastLocation, Is.EqualTo("LastLocation"));
            Assert.That(device.AvailableSpace, Is.EqualTo(1024));
            Assert.That(device.OccupiedSpace, Is.EqualTo(512));
            Assert.That(device.Name, Is.EqualTo("Dispositivo de Teste"));
            Assert.That(device.Token, Is.EqualTo("Token"));
            Assert.That(device.CreationDate, Is.Not.EqualTo(default(DateTime)));
            Assert.That(device.AlterationDate, Is.Not.EqualTo(default(DateTime)));
        });
    }

    [Test]
    public void Device_IsDeleted_Set_Correctly()
    {
        var device = new Device
        {
            IsDeleted = true
        };
        
        Assert.That(device.IsDeleted, Is.EqualTo(true));
    }
}