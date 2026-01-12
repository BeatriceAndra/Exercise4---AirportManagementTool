using AirportTool.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure
{
    public class AirportManagementContext : IdentityDbContext<ApiUser>
    {
        public AirportManagementContext(DbContextOptions<AirportManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresse { get; set; }
        public DbSet<Aircraft> Aircraft { get; set; }
        public DbSet<Airline> Airline { get; set; }
        public DbSet<Airport> Airport { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Flight> Flight { get; set; }
        public DbSet<FlightSchedule> FlightSchedule { get; set; }
        public DbSet<Gate> Gate { get; set; }
        public DbSet<Ticket> Ticket { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Aircraft>().ToTable("Aircraft");
            modelBuilder.Entity<Airline>().ToTable("Airline");
            modelBuilder.Entity<AppUser>().ToTable("AppUser");

            modelBuilder.Entity<Airport>(entity =>
            {
                entity.ToTable("Airport");

                entity.HasMany(a => a.Gates)
                      .WithOne()
                      .HasForeignKey(g => g.AirportId);
            });

            modelBuilder.Entity<Gate>().ToTable("Gate");

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.ToTable("Flight");

                entity.HasMany(f => f.Schedules)
                      .WithOne()
                      .HasForeignKey(s => s.FlightId);

                entity.Property(f => f.IsActive)
                      .HasDefaultValue(true);
            });

            modelBuilder.Entity<FlightSchedule>(entity =>
            {
                entity.ToTable("FlightSchedule");

                entity.Property(fs => fs.FlightStatusId)
                      .HasConversion<int>();

                entity.HasMany(fs => fs.Tickets)
                      .WithOne()
                      .HasForeignKey(t => t.FlightScheduleId);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(b => b.BookingStatusId)
                      .HasConversion<int>();

                entity.Property(b => b.CreatedUtc)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasMany(b => b.Tickets)
                      .WithOne()
                      .HasForeignKey(t => t.BookingId);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");

                entity.Property(t => t.Currency)
                      .HasMaxLength(3)
                      .HasDefaultValue("EUR");

                entity.Property(t => t.IsRefundable)
                      .HasDefaultValue(true);
            });
        }

    }
}
