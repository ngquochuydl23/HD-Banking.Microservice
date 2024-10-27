
using HD.Wallet.BankingResource.Service.Consumers;
using HD.Wallet.BankingResource.Service.Extensions;
using HD.Wallet.BankingResource.Service.Infrastructure;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Interceptors;
using HD.Wallet.Shared.Seedworks;
using Microsoft.EntityFrameworkCore;

namespace HD.Wallet.BankingResource.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddWebApiConfiguration(builder.Configuration);

            builder.Services.AddDbContext<BankingResourceDbContext>(options =>
            {
                options.UseMySql(
                   builder.Configuration.GetConnectionString("MySQLConnection"),
                   ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLConnection"))
                );
            });
            builder.Services.AddAutoMapperConfig<AutoMapperProfile>();
            builder.Services.AddHostedService<TransactionConsumerService>();
            var app = builder.Build();

            app.AddCommonApplicationBuilder();
            app.Run();
        }
    }
}
