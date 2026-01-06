using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("Admin");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Username)
            .HasColumnName("Username")
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(a => a.Username)
            .IsUnique();

        builder.Property(a => a.PasswordHash)
            .HasColumnName("PasswordHash")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(a => a.ModifiedDate)
            .HasColumnName("ModifiedDate");

        builder.Property(a => a.IsActive)
            .HasColumnName("IsActive")
            .HasDefaultValue(true);

        builder.Property(a => a.CreatedBy)
            .HasColumnName("CreatedBy")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasMaxLength(100);
    }
}
