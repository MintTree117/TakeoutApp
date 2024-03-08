using Api.Errors;
using Api.Extentions;
using Api.Features.Identity;
using Api.Features.Menu;
using Core.Persistence;
using Microsoft.AspNetCore.Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices( builder.Configuration );
builder.Services.AddIdentityServices( builder.Configuration );

WebApplication app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors( "CorsPolicy" );
app.UseAuthentication();
app.UseAuthorization();

app.MapReadMenuEndpoints();
app.MapIdentityEndpoints();

app.UseHttpsRedirection();

using IServiceScope scope = app.Services.CreateScope();
IServiceProvider services = scope.ServiceProvider;
var context = services.GetRequiredService<EfContext>();
var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    await MenuSeed.SeedMenuAsync( context, 10, 6, 6, 3 );
    await IdentitySeed.SeedUsersAsync( userManager );
}
catch ( Exception e )
{
    logger.LogError( e, "An error occured during seeding." );
}

app.Run();