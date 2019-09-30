using Microsoft.EntityFrameworkCore;
using SiteMonitor.Data.Entities;
using SiteMonitor.Data.Mapping;

namespace SiteMonitor.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<SiteConfiguration> SiteConfigurations { get; set; }
        public DbSet<SiteStatusCheckIntervalType> SiteStatusCheckIntervalTypes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new SiteConfigurationMap());
            modelBuilder.ApplyConfiguration(new SiteStatusCheckIntervalTypeMap());
        }
    }
}
