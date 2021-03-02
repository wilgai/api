﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api.Data;

namespace api.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("api.Entities.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("nombre")
                        .IsUnique();

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("api.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("nombre")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("nombre")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("api.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("celular");

                    b.Property<string>("correo");

                    b.Property<string>("direccion");

                    b.Property<string>("identificacion");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("sexo");

                    b.Property<string>("telefono");

                    b.HasKey("Id");

                    b.HasIndex("nombre", "correo", "identificacion")
                        .IsUnique()
                        .HasFilter("[correo] IS NOT NULL AND [identificacion] IS NOT NULL");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("api.Entities.Configuration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("celular");

                    b.Property<string>("correo");

                    b.Property<string>("direccion");

                    b.Property<string>("empresa");

                    b.Property<string>("facebook");

                    b.Property<string>("instagram");

                    b.Property<string>("instrucionOdenDeServicio");

                    b.Property<string>("logo");

                    b.Property<string>("moneda");

                    b.Property<string>("notaCotizacionFactura");

                    b.Property<string>("notaCotizacionReparacion");

                    b.Property<string>("notaFactura");

                    b.Property<string>("notaOrdenDeServicio");

                    b.Property<string>("notaServicio");

                    b.Property<string>("telefono");

                    b.Property<string>("tiktok");

                    b.Property<string>("web");

                    b.Property<string>("whatsap");

                    b.HasKey("Id");

                    b.ToTable("Configurations");
                });

            modelBuilder.Entity("api.Entities.Inventory", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Cantidad");

                    b.Property<DateTime>("Fecha");

                    b.Property<decimal>("Ganancia");

                    b.Property<decimal>("Itbis");

                    b.Property<string>("OrderNumber");

                    b.Property<decimal>("PorcientoDescuento");

                    b.Property<decimal>("PrecioCompra");

                    b.Property<decimal>("PrecioVenta");

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("api.Entities.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("brandId");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("brandId");

                    b.HasIndex("nombre")
                        .IsUnique();

                    b.ToTable("Models");
                });

            modelBuilder.Entity("api.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("OrderNumber");

                    b.Property<int?>("codigoCliente");

                    b.Property<decimal>("descuento");

                    b.Property<string>("detalle");

                    b.Property<string>("estado");

                    b.Property<DateTime>("fecha");

                    b.Property<decimal>("itbistot");

                    b.Property<string>("metPago");

                    b.Property<string>("ncf");

                    b.Property<string>("referencia");

                    b.Property<int?>("suplidor");

                    b.Property<string>("tipoDocumento");

                    b.Property<string>("tipoFactura");

                    b.Property<decimal>("totaln");

                    b.Property<string>("usuario_registro");

                    b.HasKey("Id");

                    b.HasIndex("OrderNumber")
                        .IsUnique()
                        .HasFilter("[OrderNumber] IS NOT NULL");

                    b.HasIndex("codigoCliente");

                    b.HasIndex("suplidor");

                    b.HasIndex("usuario_registro");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("api.Entities.Order_Detail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("OrderID");

                    b.Property<decimal>("PrecioVenta");

                    b.Property<int?>("RepairId");

                    b.Property<int>("cantidad");

                    b.Property<int>("codigo_articulo");

                    b.Property<string>("idFactura");

                    b.Property<string>("idInventario");

                    b.Property<decimal>("itbis");

                    b.Property<string>("referencia");

                    b.HasKey("Id");

                    b.HasIndex("OrderID");

                    b.HasIndex("RepairId");

                    b.HasIndex("codigo_articulo");

                    b.HasIndex("idInventario");

                    b.ToTable("Order_Details");
                });

            modelBuilder.Entity("api.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Activo");

                    b.Property<int>("BrandId");

                    b.Property<int>("CategoryId");

                    b.Property<int>("ModelId");

                    b.Property<int>("ProviderId");

                    b.Property<bool>("acepta_descuento");

                    b.Property<string>("codigo");

                    b.Property<string>("detalle");

                    b.Property<DateTime>("fecha_actualizacion");

                    b.Property<DateTime>("fecha_registro");

                    b.Property<string>("foto");

                    b.Property<int>("garantia");

                    b.Property<bool>("modificar_precio");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("oferta");

                    b.Property<int>("porciento_beneficio");

                    b.Property<int>("porciento_minimo");

                    b.Property<string>("referencia_interna");

                    b.Property<string>("referencia_suplidor");

                    b.Property<string>("tipo_impuesto");

                    b.Property<string>("usuario_registro");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ModelId");

                    b.HasIndex("ProviderId");

                    b.HasIndex("nombre")
                        .IsUnique();

                    b.ToTable("Products");
                });

            modelBuilder.Entity("api.Entities.Provider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("correo");

                    b.Property<string>("direccion");

                    b.Property<string>("logo");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("rnc");

                    b.Property<string>("telefono");

                    b.Property<string>("tipo");

                    b.Property<string>("web");

                    b.HasKey("Id");

                    b.HasIndex("nombre")
                        .IsUnique();

                    b.ToTable("Providers");
                });

            modelBuilder.Entity("api.Entities.Repair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("accesorios");

                    b.Property<string>("averia")
                        .IsRequired();

                    b.Property<string>("categoria");

                    b.Property<int>("codigoCliente");

                    b.Property<string>("color");

                    b.Property<decimal>("costoPieza");

                    b.Property<string>("detalle");

                    b.Property<string>("equipo")
                        .IsRequired();

                    b.Property<string>("estado");

                    b.Property<string>("fallaTecnica");

                    b.Property<DateTime>("fecha");

                    b.Property<string>("numero")
                        .IsRequired();

                    b.Property<string>("repuesto");

                    b.Property<string>("resgistradoPor");

                    b.Property<string>("serie");

                    b.Property<decimal>("total");

                    b.Property<bool>("trajoAccesorio");

                    b.HasKey("Id");

                    b.HasIndex("codigoCliente");

                    b.HasIndex("resgistradoPor");

                    b.ToTable("Repairs");
                });

            modelBuilder.Entity("api.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<string>("apellidos")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("direccion")
                        .HasMaxLength(100);

                    b.Property<bool>("estado");

                    b.Property<string>("foto");

                    b.Property<string>("identificacion")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("sexo");

                    b.Property<string>("tipo_usuario");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("api.Entities.Inventory", b =>
                {
                    b.HasOne("api.Entities.Product", "Producto")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("api.Entities.Model", b =>
                {
                    b.HasOne("api.Entities.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("brandId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("api.Entities.Order", b =>
                {
                    b.HasOne("api.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("codigoCliente")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("api.Entities.Provider", "Provider")
                        .WithMany()
                        .HasForeignKey("suplidor")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("api.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("usuario_registro")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("api.Entities.Order_Detail", b =>
                {
                    b.HasOne("api.Entities.Order", "Order")
                        .WithMany("Order_Details")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("api.Entities.Repair", "Repair")
                        .WithMany("Order_Details")
                        .HasForeignKey("RepairId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("api.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("codigo_articulo")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("api.Entities.Inventory", "Inventory")
                        .WithMany()
                        .HasForeignKey("idInventario")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("api.Entities.Product", b =>
                {
                    b.HasOne("api.Entities.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("api.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("api.Entities.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("api.Entities.Provider", "Provider")
                        .WithMany()
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("api.Entities.Repair", b =>
                {
                    b.HasOne("api.Entities.Client", "Cliente")
                        .WithMany()
                        .HasForeignKey("codigoCliente")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("api.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("resgistradoPor")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("api.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("api.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("api.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("api.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
