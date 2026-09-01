using AutoGestion.Models;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace AutoGestion.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DocType> DocTypes => Set<DocType>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<FuelLevel> FuelLevels { get; set; }
        public DbSet<ReceivingOrder> ReceivingOrders => Set<ReceivingOrder>();
        public DbSet<Inventory> Inventories => Set<Inventory>();
        public DbSet<InspectionAppointment> InspectionAppointments => Set<InspectionAppointment>();
        public DbSet<InventoryMovement> InventoryMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 2. OBLIGATORIO: Llama primero a base.OnModelCreating para mapear Identity
            base.OnModelCreating(modelBuilder);

            // Tus configuraciones existentes se mantienen igual
            modelBuilder.Entity<DocType>()
                .HasIndex(d => d.Code)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasIndex(c => new { c.CompanyId, c.Identification })
                .IsUnique()
                .HasFilter("[IsActive] = 0"); // SQL Server syntax para Soft Delete activo

            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.LicensePlate)
                .IsUnique();

            modelBuilder.Entity<Inventory>()
                .HasIndex(i => i.Code)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasOne(c => c.DocType)
                .WithMany(d => d.Clients)
                .HasForeignKey(c => c.DocTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReceivingOrder>()
                .HasOne(r => r.FuelLevel)
                .WithMany()
                .HasForeignKey(r => r.FuelLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Transmission)
                .HasConversion<string>();

            modelBuilder.Entity<InspectionAppointment>()
                .HasOne(a => a.Client)
                .WithMany()
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
