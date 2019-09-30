using System;
using System.Threading;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SiteMonitor.BusinessLogic.HelperClasses;

namespace SiteMonitor.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = BuildWebHost(args);

            StartSiteStatusCheckerThread(webHost.Services);

            webHost.Run();
        }

        private static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }
        
        private static void StartSiteStatusCheckerThread(IServiceProvider services)
        {
            var siteStatusCheckerThread = new Thread(SiteStatusCheckerThread)
            {
                IsBackground = true
            };

            siteStatusCheckerThread.Start(services);
        }

        private static void SiteStatusCheckerThread(object obj)
        {
            var serviceProvider = obj as IServiceProvider ?? 
                throw new ArgumentException($"The parameter has an invalid type {nameof(IServiceProvider)}", nameof(obj));
            
            using (var scope = serviceProvider.CreateScope())
            {
                var siteStatusChecker = scope.ServiceProvider.GetRequiredService<ISiteStatusChecker>();
                siteStatusChecker.StartChecking();
            }
        }
    }
}
