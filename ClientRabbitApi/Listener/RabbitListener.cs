﻿using ClientRabbitApi.Models;
using ClientRabbitApi.Redis;
using ClientRabbitApi.SignalR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ClientRabbitApi.Listener
{
    public class RabbitListener
    {
        private ConnectionFactory factory = null;
        private IConnection connection = null;
        private bool connecting = false;
        private IModel channel = null;
        private EventingBasicConsumer consumer = null;
        private string consumerTag = null;
        private readonly IHubContext<MyHub> _hubContext;
        private readonly IHostEnvironment _hostEnvironment;
        private Timer watchdogTimer = null;
        private readonly IConnection _connection;
        private readonly RedisService _redisService;


        public RabbitListener(IHubContext<MyHub> hubContext, IHostEnvironment hostEnvironment, RedisService redisService)
        {
            
            _hubContext = hubContext;
            _hostEnvironment = hostEnvironment;
            _redisService = redisService;
            factory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost"
            };
        }

        public void Register()
        {
            if (connecting) return;

            connecting = true;
            try
            {
                if (connection != null)
                    Unregister();

                StartWatchdog();

                connection = factory.CreateConnection();
                channel = connection.CreateModel();


                string exchangeName = "my_exchange";
                channel.ExchangeDeclare(exchange: exchangeName, type: "direct");

                //var routingKey = $"{_hostEnvironment.ApplicationName}_{Guid.NewGuid()}";

                var userId = "82dd525e-8157-4723-82de-bb460c5f5149";

                string queueName = $"user_{userId}_queue";
                channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
                channel.QueueBind(queue: queueName,
                    exchange: exchangeName,
                    routingKey: "test1");

                consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (ch, ea) =>
                {
                    var body = ea.Body;
                    try
                    {
                        var data = Encoding.UTF8.GetString(body.ToArray());
                        var message = JsonConvert.DeserializeObject<Message>(data);
                        //var email = message.Email;
                        //await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
                        //await _hubContext.Clients.Client(message.Email).SendAsync("ReceiveMessage", message);

                        var isViewEnabled = await _redisService.GetUserViewFlagAsync(message.Email);
                        if (isViewEnabled)
                        {
                            await _hubContext.Clients.Group(message.Email).SendAsync("NewMessage", message);
                        }

                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    
                };
                consumerTag = channel.BasicConsume(queueName, true, consumer);
            }
            catch (Exception ex)
            {
                Unregister();
                StartWatchdog();
                //_logger.LogError(ex.Message, null);
            }
            finally
            {
                connecting = false;
            }

        }
        public void Unregister()
        {
            try
            {
                if (watchdogTimer != null)
                    watchdogTimer.Dispose();
            }
            catch (Exception ex)
            {

            }

            try
            {
                if (channel != null)
                    channel.Dispose();
                if (connection != null)
                    connection.Dispose();
            }
            catch (Exception ex)
            {

            }

            channel = null;
            connection = null;
            consumer = null;
            consumerTag = null;
        }
        private void StartWatchdog()
        {
            watchdogTimer = new Timer(new TimerCallback(WatchdogProc), null, 60000, Timeout.Infinite);
        }

        private void WatchdogProc(object state)
        {
            if (watchdogTimer != null)
                watchdogTimer.Dispose();

            if (!connecting && connection == null)
            {
                Register();
            }
            else
            {
                StartWatchdog();
            }
        }
    }
}
