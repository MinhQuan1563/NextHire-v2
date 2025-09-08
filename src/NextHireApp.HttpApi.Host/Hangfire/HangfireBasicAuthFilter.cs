using Hangfire.Dashboard;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;

namespace NextHireApp.Hangfire
{
    public class HangfireBasicAuthFilter : IDashboardAuthorizationFilter
    {
        private readonly IConfiguration _cfg;
        public HangfireBasicAuthFilter(IServiceProvider sp) => _cfg = sp.GetRequiredService<IConfiguration>();
        public bool Authorize(DashboardContext context)
        {
            var http = context.GetHttpContext();
            var user = _cfg["Hangfire:DashboardUser"];
            var pass = _cfg["Hangfire:DashboardPass"];
            return http.Request.Headers.TryGetValue("Authorization", out var auth)
                   && auth.ToString() == "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{pass}"));
        }
    }

}
