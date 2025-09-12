using MailKit.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NextHireApp.EntityFrameworkCore;
using NextHireApp.Localization;
using NextHireApp.MultiTenancy;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Libs;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.Emailing;
using Volo.Abp.EventBus;
using Volo.Abp.Http.Client;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.MailKit;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;

namespace NextHireApp;

[DependsOn(
    typeof(NextHireAppHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(NextHireAppApplicationModule),
    typeof(NextHireAppEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpOpenIddictAspNetCoreModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpHttpClientModule),
    typeof(AbpMailKitModule),
    typeof(AbpEmailingModule),
    typeof(AbpEventBusModule)
)]
public class NextHireAppHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("NextHireApp");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });

        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder
                .AllowPasswordFlow()
                .AllowRefreshTokenFlow()
                .SetTokenEndpointUris("/connect/token")
                .RegisterScopes(
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.OfflineAccess,
                    "NextHireApp"
                )
                .AddDevelopmentEncryptionCertificate()
                .AddDevelopmentSigningCertificate()
                .UseAspNetCore()
                .EnableTokenEndpointPassthrough();
        });

    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureAuthentication(context);
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration); // OAuth2 PKCE
        ConfigureNoMvc(context);

        var services = context.Services;

        services.AddOpenIddict()
            .AddServer(options =>
            {
                // Endpoint URIs
                options.SetTokenEndpointUris("/connect/token");

                // Flows
                options.AllowPasswordFlow();
                options.AllowRefreshTokenFlow();

                // Scopes
                options.RegisterScopes(
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.OfflineAccess,
                    "NextHireApp"
                );

                // DEV: Certificates & HTTPS
                options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                // ASP.NET Core host integration
                options.UseAspNetCore()
                        .EnableTokenEndpointPassthrough();
            });

        // MailKit
        Configure<AbpMailKitOptions>(opt =>
        {
            opt.SecureSocketOption = SecureSocketOptions.SslOnConnect;
        });

        #region Localization
        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("NextHire", typeof(NextHireAppResource));
        });
        #endregion
    }

    private void ConfigureNoMvc(ServiceConfigurationContext context)
    {
        Configure<AbpMvcLibsOptions>(options =>
        {
            options.CheckLibs = false;
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });

        var auth = context.Services.GetConfiguration().GetSection("AuthServer");
        Configure<AbpRemoteServiceOptions>(opts => { opts.RemoteServices.Default = new RemoteServiceConfiguration(auth["Authority"]); });

        context.Services.AddAuthentication()
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = auth["Authority"];
                options.RequireHttpsMetadata = false;
                options.Audience = "NextHireApp";
            });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            //options.ConventionalControllers.Create(typeof(NextHireAppApplicationModule).Assembly);
            options.ConventionalControllers.Create(typeof(NextHireAppHttpApiModule).Assembly);


        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        //context.Services.AddAbpSwaggerGenWithOAuth(
        //   configuration["AuthServer:Authority"]!,
        //   new Dictionary<string, string>
        //   {
        //           {"NextHireApp", "NextHireApp API"}
        //   },
        //   options =>
        //   {
        //       options.SwaggerDoc("v1", new OpenApiInfo { Title = "NextHireApp API", Version = "v1" });
        //       options.DocInclusionPredicate((docName, description) => true);
        //       options.CustomSchemaIds(type => type.FullName);
        //       options.HideAbpEndpoints();
        //   });

        context.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "NextHireApp API", Version = "v1" });
            opt.DocInclusionPredicate((_, __) => true);
            opt.CustomSchemaIds(t => t.FullName);
            opt.HideAbpEndpoints();

            var authority = configuration["AuthServer:Authority"];

            opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                        TokenUrl = new Uri($"{authority}/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "NextHireApp", "NextHireApp API" }
                        }
                    }
                }
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new[] { "NextHireApp" }
                }
            });
        });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override async void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.MapAbpStaticAssets();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }
        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "NextHireApp API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            c.OAuthClientId(configuration["AuthServer:SwaggerClientId"] ?? "NextHireApp_Swagger");
            c.OAuthUsePkce(); // PKCE 
            c.OAuthScopes("NextHireApp");
            c.DefaultModelsExpandDepth(-1);
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
