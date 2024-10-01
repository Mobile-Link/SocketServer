using SocketServer.Entities;

namespace TestsSocketServer.UnitTests.Entities;

[TestFixture]
public class UserTests
{
    [Test]
    public void User_Valid_Properties()
    {
        var user = new User();

        Assert.IsNotNull(user.IdUser);
        Assert.IsNotNull(user.Username);
        Assert.IsNotNull(user.Email);
        Assert.IsNotNull(user.PasswordHash);
        Assert.IsNotNull(user.CreationDate);
    }
}