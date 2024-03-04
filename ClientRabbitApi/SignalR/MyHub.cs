using ClientRabbitApi.Models;
using ClientRabbitApi.Redis;
using Microsoft.AspNetCore.SignalR;

namespace ClientRabbitApi.SignalR
{
    public class MyHub : Hub
    {
        private readonly RedisService _redisService;

        public MyHub(RedisService redisService)
        {
            _redisService = redisService;
        }
        public async Task SendMessage(Message message)
        {
            //await Clients.User(message.Email).SendAsync("ReceiveMessage", message);

            await Clients.Group(message.Email).SendAsync("NewMessage", message);

            //await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task SendMessage2(Message message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task JoinUser(string email)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, email);
        }
        //Funkcja służąca do obsługi flagi, czy dany widok jest włączony i powinno się na niego wysyłać wiadomości
        public async Task UpdateUserViewFlag(string email, bool isViewEnabled)
        {
            await _redisService.UpdateUserViewFlagAsync(email, isViewEnabled);
        }
    }
}
