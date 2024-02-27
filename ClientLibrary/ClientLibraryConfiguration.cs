using ClientLibrary.Repository.Interaces;
using ClientLibrary.Repository.Repo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary
{
    public static class ClientLibraryConfiguration
    {
        public static IServiceCollection AddClientLibraryConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IMessageRepository, MessageRepository>();
            return services;
        }

    }
}
