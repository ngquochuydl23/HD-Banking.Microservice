using HD.Wallet.Shared;
using HD.Wallet.Transaction.Service.Extensions;
using HD.Wallet.Transaction.Service.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
   .AddWebApiConfiguration(builder.Configuration)
   .AddDbContext<TransactionDbContext>(builder.Configuration)
   .AddAutoMapperConfig<AutoMapperProfile>();

var app = builder.Build();
app.AddCommonApplicationBuilder();
app.Run();
