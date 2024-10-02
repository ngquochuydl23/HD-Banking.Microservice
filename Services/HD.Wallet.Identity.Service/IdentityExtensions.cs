using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static IdentityServer4.Models.IdentityResources;

namespace HD.Wallet.Identity.Service
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var identityServerConfig = configuration.GetSection("IdentityServer");

            // Register Clients
            var clientsSection = identityServerConfig.GetSection("Clients");
            var clients = new List<Client>();

            foreach (var client in clientsSection.GetChildren())
            {
                if (client.GetValue<bool>("Enabled"))
                {
                    var clientSecrets = client
                        .GetSection("ClientSecrets")
                        .GetChildren()
                        .Select(secret =>
                        {
                            var secretValue = secret.GetValue<string>("Value");
                            if (string.IsNullOrEmpty(secretValue))
                            {
                                throw new InvalidOperationException("Client secret value cannot be null or empty.");
                            }
                            return new Secret(secretValue.Sha256());
                        })
                        .ToList();

                    clients.Add(new Client
                    {
                        ClientId = client.GetValue<string>("ClientId"),
                        ClientName = client.GetValue<string>("ClientName"),
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        ClientSecrets = clientSecrets,
                        AllowedScopes = client
                            .GetSection("AllowedScopes")
                            .GetChildren()
                            .Select(x => x.Value)
                            .ToList(),
                        //AllowOfflineAccess = true,  // Enables refresh tokens
                        //RefreshTokenExpiration = TokenExpiration.Sliding,  // Sliding expiration
                        //AbsoluteRefreshTokenLifetime = 2592000,  // 30 days
                        //SlidingRefreshTokenLifetime = 1296000,   // 15 days
                        //RefreshTokenUsage = TokenUsage.OneTimeOnly
                    });
                }
            }

            // Register API Resources
            var apiResourcesSection = identityServerConfig.GetSection("ApiResources");
            var apiResources = new List<ApiResource>();

            foreach (var api in apiResourcesSection.GetChildren())
            {
                apiResources.Add(new ApiResource
                {
                    Name = api.GetValue<string>("Name"),
                    DisplayName = api.GetValue<string>("DisplayName"),
                    Scopes = api.GetSection("Scopes").GetChildren().Select(s => s.Value).ToList()
                });
            }

            var apiScopesSection = identityServerConfig.GetSection("ApiScopes");
            var apiScopes = new List<ApiScope>();

            foreach (var scope in apiScopesSection.GetChildren())
            {
                apiScopes.Add(new ApiScope
                {
                    Name = scope.GetValue<string>("Name"),
                    DisplayName = scope.GetValue<string>("DisplayName"),
                    Description = scope.GetValue<string>("Description")
                });
            }


            services.AddIdentityServer()
                .AddInMemoryClients(clients)
                .AddInMemoryApiResources(apiResources)
                .AddInMemoryApiScopes(apiScopes)
                .AddInMemoryIdentityResources(new IdentityResource[]
                {
                    new IdentityResources.OpenId(),
                })
                //.AddInMemoryPersistedGrants()
                .AddDeveloperSigningCredential()
                .AddProfileService<CustomProfileService>();
            return services;
        }
    }
}
