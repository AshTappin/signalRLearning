using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using SignalRChat.Hubs;

namespace SignalRChat.Test.Hubs
{
    public class ChatHubTest
    {
        [Test]
        public async Task SendMessage_Broadcasts()
        {
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            ChatHub hub = new ChatHub
            {
                Clients = mockClients.Object
            };

            mockClients.Setup(clients => clients.All).Returns(new Mock<IClientProxy>().Object);

            await hub.SendMessage("User A", "This is a message");

            mockClients.Verify(clients => clients.All, Times.Once);
            mockClientProxy.Verify(clientProxy => clientProxy.SendCoreAsync("ReceiveMessage",
                It.Is<object[]>(o => o.Length == 2),
            default(CancellationToken)));
        }
    }
}
