using Confluent.Kafka;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Interceptors;
using HD.Wallet.Transaction.Service.Extensions;
using HD.Wallet.Transaction.Service.ExternalServices;
using HD.Wallet.Transaction.Service.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
   .AddWebApiConfiguration(builder.Configuration)
   .AddDbContext<TransactionDbContext>(builder.Configuration)
   .AddAutoMapperConfig<AutoMapperProfile>();

builder.Services.AddHttpClient();
builder.Services.AddTransient<AccountExternalService>();
//builder.Services.AddSingleton(new ProducerBuilder<string, string>(producerConfig).Build());


var app = builder.Build();
app.AddCommonApplicationBuilder();
app.Run();
