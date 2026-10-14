using Microsoft.EntityFrameworkCore;
using BarberiaSaasApi.Core.Domain;

namespace BarberiaSaasApi.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Definimos las tablas que se mapearán en SQL Server
        public DbSet<Barberia> Barberias => Set<Barberia>();
        public DbSet<Barbero> Barberos => Set<Barbero>();
        public DbSet<Servicio> Servicios => Set<Servicio>();
        public DbSet<Cita> Citas => Set<Cita>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Evitar múltiples rutas de eliminación en cascada (Error 1785 de SQL Server)
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Barberia)
                .WithMany()
                .HasForeignKey(c => c.BarberiaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Barbero)
                .WithMany()
                .HasForeignKey(c => c.BarberoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Servicio)
                .WithMany()
                .HasForeignKey(c => c.ServicioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}