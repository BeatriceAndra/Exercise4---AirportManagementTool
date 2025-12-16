using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AirportManagement.WebApi.Models;

public partial class AirportManagementContext : DbContext
{
    public AirportManagementContext(DbContextOptions<AirportManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Aircraft> Aircraft { get; set; }

    public virtual DbSet<Airline> Airlines { get; set; }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingStatus> BookingStatuses { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<FlightSchedule> FlightSchedules { get; set; }

    public virtual DbSet<FlightStatus> FlightStatuses { get; set; }

    public virtual DbSet<Gate> Gates { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Address__3214EC074868B450");

            entity.ToTable("Address");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.Street).HasMaxLength(200);
        });

        modelBuilder.Entity<Aircraft>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Aircraft__3214EC071AC71D53");

            entity.HasIndex(e => e.TailNumber, "UQ__Aircraft__3F41D11B48DDC0CF").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Model).HasMaxLength(60);
            entity.Property(e => e.TailNumber).HasMaxLength(10);
        });

        modelBuilder.Entity<Airline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Airline__3214EC07E1A71A1C");

            entity.ToTable("Airline");

            entity.HasIndex(e => e.IATACode, "UQ__Airline__EFD6F5BE842981FD").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IATACode)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Airport__3214EC075D2E966B");

            entity.ToTable("Airport");

            entity.HasIndex(e => e.IATACode, "UQ__Airport__EFD6F5BE0092289E").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IATACode)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(120);
            entity.Property(e => e.TimeZone).HasMaxLength(64);

            entity.HasOne(d => d.Address).WithMany(p => p.Airports)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Airport_Address");
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AppUser__3214EC07C24853F3");

            entity.ToTable("AppUser");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.Name).HasMaxLength(120);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC0721AF55AD");

            entity.ToTable("Booking");

            entity.HasIndex(e => e.ConfirmationCode, "IX_Booking_ConfirmationCode").IsUnique();

            entity.HasIndex(e => e.ConfirmationCode, "UQ__Booking__1968308672C27F8C").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConfirmationCode).HasMaxLength(8);
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.BookingStatus).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.BookingStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_Status");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_User");
        });

        modelBuilder.Entity<BookingStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookingS__3214EC07515591CF");

            entity.ToTable("BookingStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Flight__3214EC071BB5B6D5");

            entity.ToTable("Flight");

            entity.HasIndex(e => new { e.AirlineId, e.FlightNumber }, "IX_Flight_Airline_FlightNumber");

            entity.HasIndex(e => new { e.OriginAirportId, e.DestinationAirportId }, "IX_Flight_Origin_Destination");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FlightNumber).HasMaxLength(8);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Airline).WithMany(p => p.Flights)
                .HasForeignKey(d => d.AirlineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_Airline");

            entity.HasOne(d => d.DefaultAircraft).WithMany(p => p.Flights)
                .HasForeignKey(d => d.DefaultAircraftId)
                .HasConstraintName("FK_Flight_DefaultAircraft");

            entity.HasOne(d => d.DestinationAirport).WithMany(p => p.FlightDestinationAirports)
                .HasForeignKey(d => d.DestinationAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_DestinationAirport");

            entity.HasOne(d => d.OriginAirport).WithMany(p => p.FlightOriginAirports)
                .HasForeignKey(d => d.OriginAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_OriginAirport");
        });

        modelBuilder.Entity<FlightSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FlightSc__3214EC07A6A15172");

            entity.ToTable("FlightSchedule", tb => tb.HasTrigger("TR_FlightSchedule_GateOverlap"));

            entity.HasIndex(e => new { e.FlightId, e.ScheduledDepartureUtc }, "IX_FlightSchedule_Flight_Departure");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.AssignedAircraft).WithMany(p => p.FlightSchedules)
                .HasForeignKey(d => d.AssignedAircraftId)
                .HasConstraintName("FK_FlightSchedule_AssignedAircraft");

            entity.HasOne(d => d.Flight).WithMany(p => p.FlightSchedules)
                .HasForeignKey(d => d.FlightId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlightSchedule_Flight");

            entity.HasOne(d => d.FlightStatus).WithMany(p => p.FlightSchedules)
                .HasForeignKey(d => d.FlightStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlightSchedule_FlightStatus");

            entity.HasOne(d => d.Gate).WithMany(p => p.FlightSchedules)
                .HasForeignKey(d => d.GateId)
                .HasConstraintName("FK_FlightSchedule_Gate");
        });

        modelBuilder.Entity<FlightStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FlightSt__3214EC073E24E2A5");

            entity.ToTable("FlightStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Gate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Gate__3214EC076FF7AB15");

            entity.ToTable("Gate");

            entity.HasIndex(e => new { e.AirportId, e.Code }, "UQ_Gate_Airport_Code").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(10);

            entity.HasOne(d => d.Airport).WithMany(p => p.Gates)
                .HasForeignKey(d => d.AirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gate_Airport");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ticket__3214EC0721AF5D7F");

            entity.ToTable("Ticket");

            entity.HasIndex(e => new { e.FlightScheduleId, e.FareClass }, "IX_Ticket_FlightSchedule_FareClass");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BasePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.FareClass).HasMaxLength(2);
            entity.Property(e => e.PassengerEmail).HasMaxLength(120);
            entity.Property(e => e.PassengerFullName).HasMaxLength(120);
            entity.Property(e => e.SeatNumber).HasMaxLength(4);
            entity.Property(e => e.Taxes).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalPrice)
                .HasComputedColumnSql("([BasePrice]+[Taxes])", true)
                .HasColumnType("decimal(11, 2)");

            entity.HasOne(d => d.Booking).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Booking");

            entity.HasOne(d => d.FlightSchedule).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.FlightScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_FlightSchedule");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
