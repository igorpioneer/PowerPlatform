using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
    public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.Discount).HasPrecision(30, 10).IsRequired();
            builder.Property(x => x.StartDateTime).IsRequired();

            builder.HasMany(c => c.Sales).WithOne(s => s.Campaign).HasForeignKey(x => x.CampaignId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(c => c.CampaignCustomers).WithOne(cc => cc.Campaign).HasForeignKey(x => x.CampaignId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
