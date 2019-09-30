using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiteMonitor.Data.Entities;

namespace SiteMonitor.Data.Mapping
{
    public class SiteConfigurationMap : IEntityTypeConfiguration<SiteConfiguration>
    {
        public void Configure(EntityTypeBuilder<SiteConfiguration> builder)
        {
            builder.ToTable("SiteConfiguration", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
            builder.Property(c => c.SiteUrl).HasColumnName(@"SiteUrl").HasColumnType("varchar(max)").IsRequired().IsUnicode(false);
            builder.Property(c => c.SiteStatusCheckIntervalTypeId).HasColumnName(@"SiteStatusCheckIntervalTypeId").HasColumnType("int").IsRequired();
            builder.Property(c => c.SiteStatusCheckInterval).HasColumnName(@"SiteStatusCheckInterval").HasColumnType("int").IsRequired();
            builder.Property(c => c.LastUpdated).HasColumnName(@"LastUpdated").HasColumnType("datetime").IsRequired();

            builder.HasOne(c => c.SiteStatusCheckIntervalType).WithMany(b => b.SiteConfigurations).HasForeignKey(c => c.SiteStatusCheckIntervalTypeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
