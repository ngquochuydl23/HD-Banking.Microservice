using HD.Wallet.Account.Service.Extensions;
using HD.Wallet.Account.Service.ExternalServices;
using HD.Wallet.Account.Service.Infrastructure;
using HD.Wallet.Account.Service.Validators;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Interceptors;

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
            

            builder.Services.AddTransient<RequestInterceptorHandler>();
            builder.Services
                .AddHttpClient("IdCardExternalApi")
				.AddHttpMessageHandler<RequestInterceptorHandler>();

       
            builder.Services.AddTransient<IdCardExternalService>();
			builder.Services.AddScoped<RequestOpenAccountValidator>();
            var app = builder.Build();

			app.AddCommonApplicationBuilder();
            app.Run();
		}
	}
}
