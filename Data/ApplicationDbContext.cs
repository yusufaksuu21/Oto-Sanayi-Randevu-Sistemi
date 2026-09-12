using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SanayiRandevu.Models;

namespace SanayiRandevu.Data
{
    // IdentityDbContext ile Identity tablolarý dahil edilmiþ DbContext
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet'ler
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<WorkingHour> WorkingHours { get; set; } = null!;
        public DbSet<BlockedDate> BlockedDates { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Vehicle.Owner iliþkisi: Owner silinirse araçlar silinsin (Cascade)
            builder.Entity<Vehicle>()
                .HasOne(v => v.Owner)
                .WithMany(u => u.Vehicles)
                .HasForeignKey(v => v.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment.Customer iliþkisi: Müþteri silinirse randevular silinsin (Cascade)
            builder.Entity<Appointment>()
                .HasOne(a => a.Customer)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment.Vehicle iliþkisi: Araç silinirse randevu silinmesin (Restrict)
            builder.Entity<Appointment>()
                .HasOne(a => a.Vehicle)
                .WithMany()
                .HasForeignKey(a => a.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment.Service iliþkisi: Service silinirse randevu silinmesin (Restrict)
            builder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment.Status string olarak saklansýn (okunurluk)
            builder.Entity<Appointment>()
                .Property(a => a.Status)
                .HasConversion<string>();
        }
    }
}
