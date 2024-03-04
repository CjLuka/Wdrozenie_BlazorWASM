using ClientLibrary;
using ClientRabbitApi;
using ClientRabbitApi.Listener;
using ClientRabbitApi.Redis;
using ClientRabbitApi.SignalR;
using Infrastructure;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RedisService>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<RabbitListener>();
builder.Services.AddMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Open",
        builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddClientLibraryConfiguration(builder.Configuration);

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}


//app.Services.GetService(UseRabbitListener)
//RabbitListener = app.Services.GetService(typeof(RabbitListener)) as RabbitListener;


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapHub<MyHub>("/chat");


app.UseRabbitListener();

app.MapControllers();
app.UseCors("Open");


app.Run();
