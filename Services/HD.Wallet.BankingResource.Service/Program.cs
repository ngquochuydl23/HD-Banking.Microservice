
using HD.Wallet.Shared;
using HD.Wallet.Shared.Interceptors;

namespace HD.Wallet.BankingResource.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddWebApiConfiguration(builder.Configuration);
            var app = builder.Build();

            app.AddCommonApplicationBuilder();
            app.Run();
        }
    }
}
