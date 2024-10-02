using SocketServer.Entities;

namespace TestsSocketServer.UnitTests.Entities;

[TestFixture]
public class UserTests
{
    [Test]
    public void User_Valid_Properties()
    {
        var user = new User()
        {
            IdUser = 1,
            Username = "Teste",
            Email = "emailDeTeste@gmail.com",
            PasswordHash = "123456"
        };

        Assert.Multiple(() =>
        {
            Assert.That(user.IdUser, Is.EqualTo(1));
            Assert.That(user.Username, Is.Not.Null);
            Assert.That(user.Email, Is.Not.Null);
            Assert.That(user.PasswordHash, Is.Not.Null);
        });
    }
}