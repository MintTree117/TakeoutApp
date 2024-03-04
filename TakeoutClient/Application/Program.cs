using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Application;
using Application.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault( args );
builder.RootComponents.Add<App>( "#app" );
builder.RootComponents.Add<HeadOutlet>( "head::after" );

builder.Services.AddScoped( sp => new HttpClient { BaseAddress = new Uri( builder.HostEnvironment.BaseAddress ) } );

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddSingleton<AppStateService>();
builder.Services.AddScoped<HttpService>();

builder.Services.AddScoped<MenuManager>();
builder.Services.AddScoped<IdentityManager>();
builder.Services.AddScoped<CartManager>();

await builder.Build().RunAsync();