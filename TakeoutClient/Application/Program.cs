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
builder.Services.AddSingleton<AppState>();
builder.Services.AddScoped<IHttpService, HttpService>();

builder.Services.AddScoped<IIdentityManager, IdentityManager>();
builder.Services.AddScoped<ICartManager, CartManager>();

await builder.Build().RunAsync();