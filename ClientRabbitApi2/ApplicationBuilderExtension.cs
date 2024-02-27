using ClientRabbitApi.Listener;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace ClientRabbitApi
{
    public static class ApplicationBuilderExtension
    {
        private static RabbitListener _rabbitListener {  get; set; }

        public static IApplicationBuilder UseRabbitListener(this WebApplication app)
        {
            _rabbitListener = app.Services.GetRequiredService<RabbitListener>();
            IApplicationLifetime life = app.Services.GetService(typeof(IApplicationLifetime)) as IApplicationLifetime;

            life.ApplicationStarted.Register(OnStarted);

            life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            _rabbitListener.Register();
        }

        private static void OnStopping()
        {
            _rabbitListener.Unregister();
        }
    }
}
