using AutoGestion.Models;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Data
{
    public class ApplicationDbContext : DbContext
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Índices Únicos
            modelBuilder.Entity<DocType>()
                .HasIndex(d => d.Code)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Identification)
                .IsUnique();


            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.LicensePlate)
                .IsUnique();

            modelBuilder.Entity<Inventory>()
                .HasIndex(i => i.Code)
                .IsUnique();

            // Configuración de Relación DocType -> Client
            modelBuilder.Entity<Client>()
                .HasOne(c => c.DocType)
                .WithMany(d => d.Clients)
                .HasForeignKey(c => c.DocTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación ReceivingOrder -> FuelLevel
            modelBuilder.Entity<ReceivingOrder>()
                .HasOne(r => r.FuelLevel)
                .WithMany()
                .HasForeignKey(r => r.FuelLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de Conversión de Enums a String (Opcional pero muy recomendado para legibilidad en BD)
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
