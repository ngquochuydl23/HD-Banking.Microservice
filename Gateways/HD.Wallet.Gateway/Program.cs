
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using System;

namespace Clothes.GatewayApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();



            if (builder.Environment.IsProduction())
            {
                builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            } else
			{
                builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }

            builder.Services.AddOcelot(builder.Configuration);
			builder.Services.AddSwaggerForOcelot(builder.Configuration);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("default", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
			{
				//app.UseSwagger();
				app.UseSwaggerForOcelotUI(opt =>
				{
					// opt.PathToSwaggerGenerator = "/swagger/docs";
				}, uiOpt =>
				{
					uiOpt.DocumentTitle = "Gateway documentation";
				});
			}

			app.UseHttpsRedirection();

            app.UseRouting();

            // Add Ocelot middleware
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Configure Ocelot
            app.UseOcelot().Wait();
            app.UseAuthorization();
			app.MapControllers();
			app.Run();
		}
	}
}
