using Api.Errors;
using Api.Extentions;
using Api.Features.Identity;
using Api.Features.Menu;
using API.Persistence;
using Microsoft.AspNetCore.Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices( builder.Configuration );
builder.Services.AddIdentityServices( builder.Configuration );

WebApplication app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.MapReadMenuEndpoints();
app.MapIdentityEndpoints();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

using IServiceScope scope = app.Services.CreateScope();
IServiceProvider services = scope.ServiceProvider;
//var context = services.GetRequiredService<EfContext>();
var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    await IdentitySeed.SeedUsersAsync( userManager );
}
catch ( Exception e )
{
    logger.LogError( e, "An error occured during seeding." );
}

app.Run();