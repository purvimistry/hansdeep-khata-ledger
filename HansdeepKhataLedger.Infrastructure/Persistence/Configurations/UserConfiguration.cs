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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(a => a.Id);
            builder.Property(a=>a.Id)
                .UseIdentityColumn();

            builder.Property(a => a.Username)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a=>a.PasswordHash)
                .IsRequired()   
                .HasMaxLength(200);

            builder.Property(a => a.Email)
                .IsRequired(false)
                .HasMaxLength(200);

            builder.Property(a => a.RoleId)
                .IsRequired();

            builder.Property(a => a.IsActive)
                .HasDefaultValue(true);

            builder.Property(a => a.CreatedOn)
                 .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(a => a.Username)
                 .IsUnique();
            builder.HasIndex(a => a.Email)
                 .IsUnique()
                 .HasFilter("[Email] IS NOT NULL");

        }
    }
}
