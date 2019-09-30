using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiteMonitor.BusinessLogic.HelperClasses;
using SiteMonitor.BusinessLogic.Services;
using SiteMonitor.Data.Contexts;
using SiteMonitor.Data.Repositories;

namespace SiteMonitor.Web
{
    public class Startup
    {
        private IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:SiteMonitor"]);
            });

            services.AddScoped<ISiteConfigurationRepository, SiteConfigurationRepository>();
            services.AddScoped<ISiteStatusCheckIntervalTypeRepository, SiteStatusCheckIntervalTypeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<ISiteAvailabilityChecker, SiteAvailabilityChecker>();
            services.AddTransient<ISiteStatusCheckIntervalConverter, SiteStatusCheckIntervalConverter>();
            services.AddSingleton<ISiteStatusChecker, SiteStatusChecker>();
            services.AddSingleton<ISiteStatusCheckScheduler, SiteStatusCheckScheduler>();
            services.AddScoped<ISiteStatusCheckService, SiteStatusCheckService>();
            services.AddScoped<ISiteConfigurationService, SiteConfigurationService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ISiteStatusCheckService siteStatusCheckService)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=SiteStatusCheck}/{action=Index}");
            });

            siteStatusCheckService.InitializeSiteStatusChecking();
        }
    }
}
