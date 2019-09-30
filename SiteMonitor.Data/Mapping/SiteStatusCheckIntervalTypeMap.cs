using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiteMonitor.Data.Entities;

namespace SiteMonitor.Data.Mapping
{
    public class SiteStatusCheckIntervalTypeMap : IEntityTypeConfiguration<SiteStatusCheckIntervalType>
    {
        public void Configure(EntityTypeBuilder<SiteStatusCheckIntervalType> builder)
        {
            builder.ToTable("SiteStatusCheckIntervalType", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedNever();
            builder.Property(c => c.Name).HasColumnName(@"Name").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(100);
        }
    }
}
