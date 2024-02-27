using BlazorApp;
using BlazorApp.Services;
using ClientLibrary;
using Infrastructure;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<MessageServicese>();
builder.Services.AddSingleton<MessageServicese>();
builder.Services.AddClientLibraryConfiguration(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<UserService>();


//builder.Services.AddSingleton<IJSRuntime, JSRuntime>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7141") });

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton(sp => new HubConnectionBuilder()
    .WithUrl(sp.GetRequiredService<HttpClient>().BaseAddress.ToString() + "chat")
    .Build());

await builder.Build().RunAsync();
