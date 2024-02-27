using BlazorApp.Models;
using System.Net.Http.Json;

namespace BlazorApp.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<User>> GetAll()
        {
            var response = await _httpClient.GetFromJsonAsync<List<User>>("api/user/user/getall");
            return response;
        }
    }
}
