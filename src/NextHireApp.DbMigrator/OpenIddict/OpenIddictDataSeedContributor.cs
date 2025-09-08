using OpenIddict.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace NextHireApp.DbMigrator.OpenIddict
{
    public class OpenIddictDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IOpenIddictApplicationManager _appManager;
        private readonly IOpenIddictScopeManager _scopeManager;

        public OpenIddictDataSeedContributor(
            IOpenIddictApplicationManager appManager,
            IOpenIddictScopeManager scopeManager)
        {
            _appManager = appManager;
            _scopeManager = scopeManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // 1) API Scope
            if (await _scopeManager.FindByNameAsync("NextHireApp") is null)
            {
                await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    Name = "NextHireApp",
                    DisplayName = "NextHireApp API",
                    Resources = { "NextHireApp" }
                });
            }

            // 2) Swagger client (Authorization Code + PKCE, no secret)
            if (await _appManager.FindByClientIdAsync("NextHireApp_Swagger") is null)
            {
                await _appManager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "NextHireApp_Swagger",
                    DisplayName = "Swagger UI",
                    ClientType = OpenIddictConstants.ClientTypes.Public, // PKCE => Public
                    ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                    RedirectUris =
                    {
                        new Uri("https://localhost:44396/swagger/oauth2-redirect.html")
                    },
                    PostLogoutRedirectUris =
                    {
                        new Uri("https://localhost:44396/swagger/index.html")
                    },
                    Permissions =
                    {
                        // endpoints
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        // flows
                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        // scopes
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        "NextHireApp",
                        // response types
                        OpenIddictConstants.Permissions.ResponseTypes.Code
                    },
                    Requirements =
                    {
                        OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange
                    }
                });
            }

            // 3) SPA client (Angular)
            //var spaClientId = "NextHireApp_SPA";
            //if (await _appManager.FindByClientIdAsync(spaClientId) is null)
            //{
            //    await _appManager.CreateAsync(new OpenIddictApplicationDescriptor
            //    {
            //        ClientId = spaClientId,
            //        DisplayName = "Angular SPA",
            //        ClientType = OpenIddictConstants.ClientTypes.Public,
            //        RedirectUris =
            //    {
            //        new Uri("https://localhost:4200/signin-oidc")
            //    },
            //        PostLogoutRedirectUris =
            //    {
            //        new Uri("https://localhost:4200/signout-callback-oidc")
            //    },
            //        Permissions =
            //    {
            //        OpenIddictConstants.Permissions.Endpoints.Authorization,
            //        OpenIddictConstants.Permissions.Endpoints.Token,
            //        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
            //        OpenIddictConstants.Permissions.ResponseTypes.Code,
            //        "NextHireApp", OpenIddictConstants.Permissions.Scopes.Profile, OpenIddictConstants.Permissions.Scopes.Email
            //    },
            //        Requirements =
            //    {
            //        OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange
            //    }
            //    });
            //}

            // 4) Password Flow client (for direct username/password login)
            if (await _appManager.FindByClientIdAsync("NextHireApp_PasswordFlow") is null)
            {
                await _appManager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "NextHireApp_PasswordFlow",
                    ClientSecret = "secret123!",
                    DisplayName = "Password Flow Client",
                    ClientType = OpenIddictConstants.ClientTypes.Confidential,
                    Permissions =
                    {
                        // endpoints
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        // flows
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        // scopes
                        "NextHireApp",
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Email
                    }
                });
            }
        }
    }

}
