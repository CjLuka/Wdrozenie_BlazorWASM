using ClientRabbitApi.Models;
using ClientRabbitApi.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ClientRabbitApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	
	public class MessageController : ControllerBase
	{
        private readonly IHubContext<MyHub> _hubContext;
        private readonly IMemoryCache _memoryCache;

        public MessageController(IHubContext<MyHub> hubContext, IMemoryCache memoryCache)
        {
            _hubContext = hubContext;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessage1()
        {
            var message = _memoryCache.GetOrCreate("UserMessages", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10); // Ustawienie czasu ważności w pamięci podręcznej
                return new List<Message>();
            });

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "RabbitMq.01",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += async (model, ea) =>
                {
                    var jsonMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var receivedUserMessages = JsonConvert.DeserializeObject<Message>(jsonMessage);

                    message.Add(receivedUserMessages);

                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", receivedUserMessages);
                };

                channel.BasicConsume(queue: "RabbitMq.01", autoAck: true, consumer: consumer);
            }

            return Ok(JsonConvert.SerializeObject(message));
        }

        
        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> GetMessage2()
        {
            var message = _memoryCache.GetOrCreate("UserMessages2", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10); // Ustawienie czasu ważności w pamięci podręcznej
                return new List<Message>();
            });

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Test2",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += async (model, ea) =>
                {
                    var jsonMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var receivedUserMessages = JsonConvert.DeserializeObject<Message>(jsonMessage);

                    message.Add(receivedUserMessages);

                    await _hubContext.Clients.All.SendAsync("ReceiveMessage2", receivedUserMessages);
                };

                channel.BasicConsume(queue: "Test2", autoAck: true, consumer: consumer);
            }

            return Ok(JsonConvert.SerializeObject(message));
        }
    }
}
