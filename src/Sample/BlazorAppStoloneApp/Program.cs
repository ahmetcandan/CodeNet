using CodeNetUI_Example;
using CodeNetUI_Example.Configurations;
using CodeNetUI_Example.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { });
builder.Services.AddScoped<ApiClientService>();
builder.Services.AddScoped<BackgroundService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<LocalStorageManager>();
builder.Services.AddSingleton(sp => new HubConnectionBuilder()
    .WithUrl($"{AppSettings.BackgroundJobBaseUrl}/jobEvents")
    .Build());

await builder.Build().RunAsync();
