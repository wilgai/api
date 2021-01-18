using api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace api.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Model> Models { get; set; }

        public DbSet<Order_Detail> Order_Details { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Configuration> Configurations { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Repair> Repairs { get; set; }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Client> Clients { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasIndex(t => t.nombre)
                .IsUnique();

            modelBuilder.Entity<Product>()
               .HasIndex(t => t.nombre)
               .IsUnique();

            modelBuilder.Entity<Provider>()
               .HasIndex(t => t.nombre)
               .IsUnique();

            modelBuilder.Entity<Brand>()
               .HasIndex(t => t.nombre)
               .IsUnique();

            modelBuilder.Entity<Model>()
               .HasIndex(t => t.nombre)
               .IsUnique();



            modelBuilder.Entity<Client>(dep =>
            {
                dep.HasIndex("nombre", "correo", "identificacion").IsUnique();

            });




        }
    }
}
