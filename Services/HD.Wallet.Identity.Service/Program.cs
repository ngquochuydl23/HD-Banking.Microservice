
using HD.Wallet.Identity.ExternalServices;
using HD.Wallet.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddWebApiConfiguration(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddTransient<UserExternalService>();


var app = builder.Build();
app.AddCommonApplicationBuilder();
app.Run();
