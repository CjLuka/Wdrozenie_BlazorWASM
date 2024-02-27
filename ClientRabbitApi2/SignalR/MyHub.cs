using ClientRabbitApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace ClientRabbitApi.SignalR
{
    public class MyHub : Hub
    {
        public async Task SendMessage(Message message)
        {
            //await Clients.User(message.Context).SendAsync("ReceiveMessage", message);
            //await Clients.Client(message.Context).SendAsync("ReceiveMessage", message);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task SendMessage2(Message message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", message.UserId, message.Context);
            await Clients.All.SendAsync("ReceiveMessage2", message);
        }
    }
}
