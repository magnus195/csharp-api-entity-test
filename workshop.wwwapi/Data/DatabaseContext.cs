using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using workshop.wwwapi.Enums;
using workshop.wwwapi.Models;

namespace workshop.wwwapi.Data;

public class DatabaseContext : DbContext
{
    private readonly string _connectionString;

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
        //Database.EnsureCreated();
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>().HasKey(a => new { a.DoctorId, a.PatientId });

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId);
        
        //TODO: Seed Data Here
        modelBuilder.Entity<Patient>().HasData(
            new Patient { Id = 1, FullName = "John Doe" },
            new Patient { Id = 2, FullName = "Jane Doe" }
        );
        
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { Id = 1, FullName = "Dr. John Smith" },
            new Doctor { Id = 2, FullName = "Dr. Jane Smith" }
        );
        
        modelBuilder.Entity<Appointment>().HasData(
            new Appointment { Type = AppointmentType.Online, DoctorId = 1, PatientId = 1, Booking = DateTime.UtcNow - TimeSpan.FromMinutes(60) },
            new Appointment { Type = AppointmentType.InPerson, DoctorId = 2, PatientId = 2, Booking = DateTime.UtcNow - TimeSpan.FromMinutes(30) }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
        optionsBuilder.LogTo(message => Debug.WriteLine(message));
    }
}