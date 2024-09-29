using HD.Wallet.Account.Service.Extensions;
using HD.Wallet.Account.Service.Infrastructure;
using HD.Wallet.Shared;

namespace HD.Wallet.Account.Service
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services
			   .AddWebApiConfiguration(builder.Configuration)
			   .AddDbContext<HdWalletAccountDbContext>(builder.Configuration)
			   .AddAutoMapperConfig<AutoMapperProfile>();

			var app = builder.Build();
			app.AddCommonApplicationBuilder();
			app.Run();
		}
	}
}
