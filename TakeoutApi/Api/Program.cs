using Api.Features.ViewMenu;
using API.Persistence;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EfContext>( ( options ) =>
{
    options.UseSqlite( builder.Configuration.GetConnectionString( "DefaultConnection" ) );
} );

WebApplication app = builder.Build();

app.MapReadMenuEndpoints();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();