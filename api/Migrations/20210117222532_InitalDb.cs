using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class InitalDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    identificacion = table.Column<string>(maxLength: 20, nullable: false),
                    nombre = table.Column<string>(maxLength: 50, nullable: false),
                    apellidos = table.Column<string>(maxLength: 50, nullable: false),
                    direccion = table.Column<string>(maxLength: 100, nullable: true),
                    foto = table.Column<string>(nullable: true),
                    sexo = table.Column<string>(nullable: true),
                    estado = table.Column<string>(nullable: true),
                    tipo_usuario = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(maxLength: 50, nullable: false),
                    direccion = table.Column<string>(nullable: true),
                    identificacion = table.Column<string>(nullable: true),
                    correo = table.Column<string>(nullable: true),
                    celular = table.Column<string>(nullable: true),
                    telefono = table.Column<string>(nullable: true),
                    sexo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    empresa = table.Column<string>(nullable: true),
                    web = table.Column<string>(nullable: true),
                    correo = table.Column<string>(nullable: true),
                    telefono = table.Column<string>(nullable: true),
                    celular = table.Column<string>(nullable: true),
                    whatsap = table.Column<int>(nullable: false),
                    facebook = table.Column<string>(nullable: true),
                    instagram = table.Column<string>(nullable: true),
                    tiktok = table.Column<string>(nullable: true),
                    logo = table.Column<string>(nullable: true),
                    notaFactura = table.Column<string>(nullable: true),
                    notaOrdenDeServicio = table.Column<string>(nullable: true),
                    notaServicio = table.Column<string>(nullable: true),
                    instrucionOdenDeServicio = table.Column<string>(nullable: true),
                    moneda = table.Column<string>(nullable: true),
                    direccion = table.Column<string>(nullable: true),
                    notaCotizacionFactura = table.Column<string>(nullable: true),
                    notaCotizacionReparacion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(maxLength: 50, nullable: false),
                    marca = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(maxLength: 50, nullable: false),
                    rnc = table.Column<string>(nullable: true),
                    direccion = table.Column<string>(nullable: true),
                    telefono = table.Column<string>(nullable: true),
                    correo = table.Column<string>(nullable: true),
                    web = table.Column<string>(nullable: true),
                    tipo = table.Column<string>(nullable: true),
                    logo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    usuario_registroId = table.Column<string>(nullable: true),
                    tipoDocumento = table.Column<string>(nullable: true),
                    codigoClienteId = table.Column<int>(nullable: true),
                    ncf = table.Column<string>(nullable: true),
                    referencia = table.Column<string>(nullable: true),
                    descuento = table.Column<decimal>(nullable: false),
                    detalle = table.Column<string>(nullable: true),
                    estado = table.Column<int>(nullable: false),
                    totaln = table.Column<decimal>(nullable: false),
                    itbistot = table.Column<decimal>(nullable: false),
                    fecha = table.Column<DateTime>(nullable: false),
                    OrderNumber = table.Column<string>(nullable: true),
                    metPago = table.Column<string>(nullable: true),
                    tipoFactura = table.Column<string>(nullable: true),
                    suplidor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Clients_codigoClienteId",
                        column: x => x.codigoClienteId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_usuario_registroId",
                        column: x => x.usuario_registroId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Repairs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    numero = table.Column<string>(nullable: false),
                    codigoClienteId = table.Column<int>(nullable: true),
                    resgistradoPorId = table.Column<string>(nullable: true),
                    detalle = table.Column<string>(nullable: true),
                    total = table.Column<decimal>(nullable: false),
                    fecha = table.Column<DateTime>(nullable: false),
                    estado = table.Column<string>(nullable: true),
                    categoria = table.Column<string>(nullable: true),
                    equipo = table.Column<string>(nullable: false),
                    serie = table.Column<string>(nullable: true),
                    color = table.Column<string>(nullable: true),
                    trajoAccesorio = table.Column<bool>(nullable: false),
                    accesorios = table.Column<string>(nullable: true),
                    averia = table.Column<string>(nullable: true),
                    fallaTecnica = table.Column<string>(nullable: false),
                    costoPieza = table.Column<decimal>(nullable: false),
                    repuesto = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repairs_Clients_codigoClienteId",
                        column: x => x.codigoClienteId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Repairs_AspNetUsers_resgistradoPorId",
                        column: x => x.resgistradoPorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(maxLength: 50, nullable: false),
                    codigo_suplidorId = table.Column<int>(nullable: true),
                    usuario_registro = table.Column<int>(nullable: false),
                    fecha_registro = table.Column<DateTime>(nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(nullable: false),
                    tipo_impuesto = table.Column<string>(nullable: true),
                    estado = table.Column<bool>(nullable: false),
                    codigo_categoriaId = table.Column<int>(nullable: true),
                    referencia_interna = table.Column<string>(nullable: true),
                    referencia_suplidor = table.Column<string>(nullable: true),
                    foto = table.Column<string>(nullable: true),
                    oferta = table.Column<bool>(nullable: false),
                    modificar_precio = table.Column<bool>(nullable: false),
                    acepta_descuento = table.Column<bool>(nullable: false),
                    detalle = table.Column<string>(nullable: true),
                    codigo_marcaId = table.Column<int>(nullable: true),
                    porciento_beneficio = table.Column<int>(nullable: false),
                    porciento_minimo = table.Column<int>(nullable: false),
                    modeloId = table.Column<int>(nullable: true),
                    codigo = table.Column<string>(nullable: true),
                    garantia = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_codigo_categoriaId",
                        column: x => x.codigo_categoriaId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Brands_codigo_marcaId",
                        column: x => x.codigo_marcaId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Providers_codigo_suplidorId",
                        column: x => x.codigo_suplidorId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Models_modeloId",
                        column: x => x.modeloId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdProductoId = table.Column<int>(nullable: true),
                    PrecioCompra = table.Column<decimal>(nullable: false),
                    Ganancia = table.Column<decimal>(nullable: false),
                    PrecioVenta = table.Column<decimal>(nullable: false),
                    Descuento = table.Column<decimal>(nullable: false),
                    PorcientoDescuento = table.Column<decimal>(nullable: false),
                    Cantidad = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_IdProductoId",
                        column: x => x.IdProductoId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order_Details",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    codigo_articuloId = table.Column<int>(nullable: true),
                    cantidad = table.Column<int>(nullable: false),
                    precio = table.Column<decimal>(nullable: false),
                    itbis = table.Column<decimal>(nullable: false),
                    IdInventario = table.Column<int>(nullable: false),
                    referencia = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Details_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Details_Products_codigo_articuloId",
                        column: x => x.codigo_articuloId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_nombre",
                table: "Brands",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_nombre",
                table: "Categories",
                column: "nombre",
                unique: true,
                filter: "[nombre] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_nombre_correo_identificacion",
                table: "Clients",
                columns: new[] { "nombre", "correo", "identificacion" },
                unique: true,
                filter: "[correo] IS NOT NULL AND [identificacion] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_IdProductoId",
                table: "Inventories",
                column: "IdProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_nombre",
                table: "Models",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_Details_OrderId",
                table: "Order_Details",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Details_codigo_articuloId",
                table: "Order_Details",
                column: "codigo_articuloId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_codigoClienteId",
                table: "Orders",
                column: "codigoClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_usuario_registroId",
                table: "Orders",
                column: "usuario_registroId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_codigo_categoriaId",
                table: "Products",
                column: "codigo_categoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_codigo_marcaId",
                table: "Products",
                column: "codigo_marcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_codigo_suplidorId",
                table: "Products",
                column: "codigo_suplidorId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_modeloId",
                table: "Products",
                column: "modeloId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_nombre",
                table: "Products",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Providers_nombre",
                table: "Providers",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_codigoClienteId",
                table: "Repairs",
                column: "codigoClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_resgistradoPorId",
                table: "Repairs",
                column: "resgistradoPorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Configurations");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Order_Details");

            migrationBuilder.DropTable(
                name: "Repairs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Models");
        }
    }
}
