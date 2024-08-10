using BlazorAppStoloneApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { });
builder.Services.AddSingleton(sp => new HubConnectionBuilder()
    .WithUrl($"https://localhost:5011/jobEvents")
    .Build());

await builder.Build().RunAsync();
