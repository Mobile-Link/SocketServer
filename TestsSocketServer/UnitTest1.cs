using Microsoft.AspNetCore.Mvc;
using SocketServer.Controllers;
using Moq;

namespace TestsSocketServer;

[TestFixture]
public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
    
    [Test]
    public void Test2()
    {
        var mock = new Mock<ConnectionController>();
        
        var controller = mock.Object;
        
        var result = controller.GetConnections(); 
        
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.AreEqual("aujdb", (result as OkObjectResult).Value);
    }
}