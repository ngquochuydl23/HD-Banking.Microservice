﻿using AutoMapper;
using HD.Wallet.Shared.Seedworks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Logging;
using HD.Wallet.Shared.Middlewares;
using HD.Wallet.Shared.Settings.JwtSetting;
using Microsoft.Extensions.Hosting;
using Redis.OM;
using Confluent.Kafka;
using IdentityServer4.Models;
using IdentityModel.Client;
using System.Reflection;
using HD.Wallet.Shared.Attributes;
using System;


namespace HD.Wallet.Shared
{
	public static class CommonExtensions
	{
		public static IServiceCollection AddWebApiConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services
				 .AddControllers()
				 .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
			services.AddCors();
			services.AddEndpointsApiExplorer();
			services.AddHttpContextAccessor();
			services.AddHttpClient();
            services.AddScoped<PinRequiredAttribute>();
            services.Configure<ForwardedHeadersOptions>(options =>
			{
				options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
			});

			services
				 .AddTransient<IHttpContextAccessor, HttpContextAccessor>()
				 .AddDefaultOpenApi(configuration)
				 .AddDefaultAuthentication(configuration)
				 .AddLogger(configuration)
				 .AddJwtExtension(configuration)
				 .AddRedisConfiguration(configuration);

			return services;
		}

		public static IServiceCollection AddDbContext<T>(this IServiceCollection services, IConfiguration configuration) where T : DbContext
		{
            var environment = configuration["ASPNETCORE_ENVIRONMENT"] ?? "Production";
            services.AddDbContext<T>(x =>
			{
                if (!environment.Equals("Development", StringComparison.OrdinalIgnoreCase))
                {
                    x.EnableSensitiveDataLogging();
                }
                x.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

			});
			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
			AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

			services.AddScoped<DbContext, T>();
			services.AddTransient(typeof(IEfRepository<,>), typeof(EfRepository<,>));
			services.AddTransient<IUnitOfWork, UnitOfWork>();
			return services;
		}

		public static IServiceCollection AddAutoMapperConfig<TProfile>(this IServiceCollection services) where TProfile : Profile
		{
			services.AddAutoMapper(typeof(TProfile).Assembly);
			return services;
		}
		public static IServiceCollection AddLogger(this IServiceCollection services, IConfiguration configuration)
		{
			var section = configuration.GetSection("Seq");
			if (!section.Exists())
				return services;

			services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.AddSeq(section);
			});
			return services;
		}

		public static IServiceCollection AddDefaultOpenApi(this IServiceCollection services, IConfiguration configuration)
		{
			var section = configuration.GetSection("OpenApi");
			if (!section.Exists())
				return services;

			var document = section.GetRequiredSection("Document");

			if (!document.Exists())
				return services;

			var version = document.GetRequiredValue("Version") ?? "v1";
			var title = document.GetRequiredValue("Title");
			var description = document.GetRequiredValue("Description");

			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc(version, new OpenApiInfo
				{
					Title = title,
					Version = version,
					Description = description
				});

				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "Insira o token JWT desta maneira: Bearer {seu token}",
					Name = "Authorization",
					Scheme = "bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
					{
					  {
						new OpenApiSecurityScheme
						{
						  Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
						  new string[] {}
						}
					});
				options.OperationFilter<AuthorizeCheckOperationFilter>();

            });
			return services;
		}

		public static IServiceCollection AddDefaultAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
            var identifierSection = configuration.GetSection("Identity");

            if (!identifierSection.Exists())
                return services;

            services
              .AddAuthentication(option =>
              {
                  option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                  option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
              })
              .AddJwtBearer(options =>
              {
                  var secretKey = identifierSection.GetRequiredValue("SecretKey");

                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = false,
                      ValidateAudience = false,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                  };
              });
            return services;
		}
       

        public static IServiceCollection AddJwtExtension(this IServiceCollection services, IConfiguration configuration)
		{
			var section = configuration.GetSection("Identity");
			if (!section.Exists())
				return services;

			services.Configure<JwtSettings>(section);
			services.AddSingleton<IJwtExtension, JwtExtension>();
			return services;
		}

		public static IServiceCollection AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			var section = configuration.GetSection("Redis");
			if (!section.Exists())
				return services;

			var redisConnectionString = section.GetRequiredValue("Title");
			services.AddSingleton(new RedisConnectionProvider(configuration[redisConnectionString]));
			services.AddTransient(typeof(IRedisRepository<,>), typeof(RedisRepository<,>));
			return services;
		}

		public static IApplicationBuilder AddCommonApplicationBuilder(this WebApplication app)
		{
			if (!app.Environment.IsProduction())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseMiddleware<ErrorHandlingMiddleWare>();
			app.UseHttpsRedirection();
            app.UseAuthentication();
			app.UseAuthorization();
			app.MapControllers();


			return app;
		}

		public static string GetRequiredValue(this IConfiguration configuration, string name) =>
			configuration[name] ?? throw new InvalidOperationException($"Configuration missing value for: {(configuration is IConfigurationSection s ? s.Path + ":" + name : name)}");
	}
}