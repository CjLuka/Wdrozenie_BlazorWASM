using ClientRabbitApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace ClientRabbitApi.SignalR
{
    public class MyHub : Hub
    {
        public async Task SendMessage(Message message)
        {
            //await Clients.User(message.Email).SendAsync("ReceiveMessage", message);

            await Clients.Group(message.Email).SendAsync("NewMessage", message);

            //await Clients.All.SendAsync("ReceiveMessage", message);
        }
        //public override async Task OnConnectedAsync()
        //{
        //    var connectionId = Context.ConnectionId;
        //    // Tutaj możesz zrobić coś z connectionId, na przykład zapisywać go w bazie danych
        //    await base.OnConnectedAsync();
        //}
        //public async Task SendMessage(Message message)
        //{
        //    var connectionId = Context.ConnectionId;
        //    await Clients.Client(connectionId).SendAsync("NewMessage", message);
        //}
        public async Task SendMessage2(Message message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task JoinUser(string email)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, email);
        }

    }
}
