using StackExchange.Redis;

namespace ClientRabbitApi.Redis
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _connection;

        public RedisService()
        {
            _connection = ConnectionMultiplexer.Connect("localhost:6379");
        }

        public IDatabase GetDatabase()
        {
            return _connection.GetDatabase();
        }

        public async Task SetUserViewFlagAsync(string email, bool isViewEnabled)
        {
            var db = GetDatabase();
            await db.StringSetAsync($"user:{email}:view", isViewEnabled);
        }

        public async Task<bool> GetUserViewFlagAsync(string email)
        {
            var db = GetDatabase();
            return (bool)await db.StringGetAsync($"user:{email}:view");
        }
        public async Task UpdateUserViewFlagAsync(string email, bool isViewEnabled)
        {
            await SetUserViewFlagAsync(email, isViewEnabled);
        }
    }
}
