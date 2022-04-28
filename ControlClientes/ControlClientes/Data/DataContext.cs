using ControlClientes.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ControlClientes.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Direccion> Direcciones { get; set; }

        public DbSet<ConocimientoDedesarrollo> Conocimientos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>()
                .HasIndex(t => t.Identificacion)
                .IsUnique();
        }

    }
}
