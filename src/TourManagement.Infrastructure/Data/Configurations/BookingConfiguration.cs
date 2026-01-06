using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("booking");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("booking_id")
            .ValueGeneratedOnAdd();

        builder.Property(b => b.TourId)
            .HasColumnName("TourId");

        builder.Property(b => b.UserId)
            .HasColumnName("UserId");

        builder.Property(b => b.TourName)
            .HasColumnName("TOUR_NAME")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Place)
            .HasColumnName("PLACE")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.FirstName)
            .HasColumnName("FirstName")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.BookingDate)
            .HasColumnName("BookingDate")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.ModifiedDate)
            .HasColumnName("ModifiedDate");

        builder.Property(b => b.IsActive)
            .HasColumnName("IsActive")
            .HasDefaultValue(true);

        builder.Property(b => b.CreatedBy)
            .HasColumnName("CreatedBy")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasMaxLength(100);

        builder.HasOne(b => b.Tour)
            .WithMany(t => t.Bookings)
            .HasForeignKey(b => b.TourId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
