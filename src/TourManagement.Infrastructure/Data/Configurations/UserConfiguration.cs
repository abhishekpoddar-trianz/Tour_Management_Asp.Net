using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Userinfo");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.FirstName)
            .HasColumnName("FirstName")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .HasColumnName("LastName")
            .HasMaxLength(100);

        builder.Property(u => u.PhoneNumber)
            .HasColumnName("PhoneNumber")
            .HasMaxLength(20);

        builder.Property(u => u.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.ModifiedDate)
            .HasColumnName("ModifiedDate");

        builder.Property(u => u.IsActive)
            .HasColumnName("IsActive")
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedBy)
            .HasColumnName("CreatedBy")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasMaxLength(100);

        builder.HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
