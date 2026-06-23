using HansdeepKhataLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.MobileNumber)
                .HasMaxLength(15);

            builder.Property(x => x.AlternateMobileNumber)
                .HasMaxLength(15);

            builder.Property(x => x.AdvanceBalance)
               .HasPrecision(18, 2);

            builder.Property(x => x.Notes)
                .HasMaxLength(100);

            builder.Property(a => a.IsActive)
              .HasDefaultValue(true);

            builder.HasOne(x => x.Village)
                .WithMany(x => x.Customers)
                .HasForeignKey(x => x.VillageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Area)
                .WithMany(x => x.Customers)
                .HasForeignKey(x => x.AreaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
