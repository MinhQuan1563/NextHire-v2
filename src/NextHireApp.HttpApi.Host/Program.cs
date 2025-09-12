using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;

namespace NextHireApp;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            //.WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
            .CreateLogger();

        try
        {
            Log.Information("Starting NextHireApp.HttpApi.Host.");
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                ContentRootPath = Directory.GetCurrentDirectory(),
                WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
            });
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();

#if DEBUG
            AppDomain.CurrentDomain.AssemblyLoad += (s, e) =>
            {
                try
                {
                    _ = e.LoadedAssembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Console.WriteLine($"[TypeLoadErr] {e.LoadedAssembly.FullName}");
                    foreach (var le in ex.LoaderExceptions)
                    {
                        Console.WriteLine("  -> " + le?.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AsmErr] {e.LoadedAssembly.FullName}: {ex.Message}");
                }
            };
#endif

            await builder.AddApplicationAsync<NextHireAppHttpApiHostModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
