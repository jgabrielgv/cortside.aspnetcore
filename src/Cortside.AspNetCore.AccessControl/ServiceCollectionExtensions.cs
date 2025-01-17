using System.IdentityModel.Tokens.Jwt;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.AspNetCore.AccessControl {
    public static class ServiceCollectionExtensions {
        /// <summary>
        /// Adds the access control using IdentityServer and PolicyServer. Sections named IdentityServer and PolicyServer
        /// are expected to be found in configuration.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddAccessControl(this IServiceCollection services, IConfiguration configuration) {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var identityServerConfiguration = configuration.GetSection("IdentityServer").Get<IdentityServerConfiguration>();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options => {
                    // base-address of your identityserver
                    options.Authority = identityServerConfiguration.Authority;
                    options.RequireHttpsMetadata = identityServerConfiguration.RequireHttpsMetadata;
                    options.RoleClaimType = "role";
                    options.NameClaimType = "name";

                    // name of the API resource
                    options.ApiName = identityServerConfiguration.ApiName;
                    options.ApiSecret = identityServerConfiguration.ApiSecret;

                    options.EnableCaching = identityServerConfiguration.EnableCaching;
                    options.CacheDuration = identityServerConfiguration.CacheDuration;
                });

            // policy server
            configuration["PolicyServer:TokenClient:Authority"] = identityServerConfiguration.Authority;
            configuration["PolicyServer:TokenClient:ClientId"] = identityServerConfiguration.Authentication.ClientId;
            configuration["PolicyServer:TokenClient:ClientSecret"] = identityServerConfiguration.Authentication.ClientSecret;
            services.AddPolicyServerRuntimeClient(configuration.GetSection("PolicyServer"))
                .AddAuthorizationPermissionPolicies();

            return services;
        }
    }
}
