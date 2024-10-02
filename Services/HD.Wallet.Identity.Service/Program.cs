using HD.Wallet.Identity.Service;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.AddDebug();
});
var app = builder.Build();


if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseIdentityServer(); 
app.UseAuthorization();
app.MapControllers();

app.Run();
