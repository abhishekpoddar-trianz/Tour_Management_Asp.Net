using Microsoft.EntityFrameworkCore;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Data.Configurations;

namespace TourManagement.Infrastructure.Data;

public class TourManagementDbContext : DbContext
{
    public TourManagementDbContext(DbContextOptions<TourManagementDbContext> options) : base(options)
    {
    }

    public DbSet<Tour> Tours { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Admin> Admins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TourConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
        modelBuilder.ApplyConfiguration(new AdminConfiguration());
    }
}
