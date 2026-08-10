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
    public class LedgerEntryConfiguration : IEntityTypeConfiguration<LedgerEntry>
    {
        public void Configure(EntityTypeBuilder<LedgerEntry> builder)
        {
            builder.ToTable("LedgerEntries");
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(l => l.Description)
                .HasMaxLength(500);
            builder.Property(l => l.EntryDate)
                .IsRequired();
            builder.Property(l => l.EntryType)
                .HasConversion<int>()
                .IsRequired();
            builder.HasOne(l => l.Customer)
                .WithMany(c => c.LedgerEntries)
                .HasForeignKey(l => l.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
