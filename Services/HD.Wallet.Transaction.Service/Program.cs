using Confluent.Kafka;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Interceptors;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Transaction.Service.Extensions;
using HD.Wallet.Transaction.Service.ExternalServices;
using HD.Wallet.Transaction.Service.Infrastructure;
using HD.Wallet.Transaction.Service.Producers.TransactionProducer;

var builder = WebApplication.CreateBuilder(args);

builder.Services
   .AddWebApiConfiguration(builder.Configuration)
   .AddDbContext<TransactionDbContext>(builder.Configuration)
   .AddAutoMapperConfig<AutoMapperProfile>();

builder.Services.AddHttpClient();
builder.Services.AddTransient<AccountExternalService>();
builder.Services.AddTransient<BankExternalService>();
builder.Services.AddTransient(typeof(ITransactionProducer), typeof(TransactionProducer));


var app = builder.Build();
app.AddCommonApplicationBuilder();
app.Run();
