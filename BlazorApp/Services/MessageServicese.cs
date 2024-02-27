
using BlazorApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;

namespace BlazorApp.Services
{
    public class MessageServicese
    {
        //private readonly HttpClient _httpClient;
        //public MessageServicese(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}

        //public async Task <List<Message>> GetMessagesAsync()
        //{
        //    var apiUrl = "https://localhost:7141/api/Message";

        //    try
        //    {
        //        var message = await _httpClient.GetFromJsonAsync<List<Message>>(apiUrl);
        //        return message;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        return new List<Message>();
        //    }
        //}

        //-----------------------------------------
        //private readonly HttpClient _httpClient;
        //public event Action<List<Message>> OnMessageReceived; // Zdarzenie do obsługi otrzymanych wiadomości

        //public MessageServicese(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}

        //public async Task<List<Message>> GetMessagesAsync()
        //{
        //    var apiUrl = "https://localhost:7141/api/Message";

        //    try
        //    {
        //        var messages = await _httpClient.GetFromJsonAsync<List<Message>>(apiUrl);

        //        // Jeśli istnieje obsługiwane zdarzenie OnMessageReceived, wywołaj je
        //        OnMessageReceived?.Invoke(messages);

        //        return messages;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        return new List<Message>();
        //    }
        //}

        //----------------
        private readonly HttpClient _httpClient;
        private readonly HubConnection _hubConnection;
        public event Action<List<Message>> OnMessageReceived;

        public MessageServicese(HttpClient httpClient, HubConnection hubConnection)
        {
            _httpClient = httpClient;
            _hubConnection = hubConnection;

            _hubConnection.On<Message>("ReceiveMessage", (message) =>
            {
                // Przetwarzanie otrzymanej wiadomości
                OnMessageReceived?.Invoke(new List<Message> { message });
            });
        }

        public async Task StartAsync()
        {
            await _hubConnection.StartAsync();
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
            var apiUrl = "https://localhost:7141/api/Message";

            try
            {
                var messages = await _httpClient.GetFromJsonAsync<List<Message>>(apiUrl);
                return messages;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Message>();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _hubConnection.DisposeAsync();
        }
    }
}
