using api.Data;
using api.Entities;
using api.Helper;
using api.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace api.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class RepairController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public RepairController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRepairs()
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso." });
            }

            try
            {
                List<Repair> repairs = await _context.Repairs
                 .Include(c => c.Cliente)
                 .Include(u => u.User)
                 .OrderByDescending(o => o.fecha)
                 .ToListAsync();
                var results = new
                {
                    IsSuccess = true,
                    repairs
                };
                return Ok(results);
            }
            catch (Exception exception)
            {

                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }

        }
        [HttpPost]
        public async Task<IActionResult> PostRepair([FromBody] Repair request)
        {

            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso." });
            }
            try
            {

                Repair repair = new Repair
                {
                    fecha = DateTime.UtcNow,
                    User = await _context.Users.FindAsync(request.resgistradoPor),
                    codigoCliente = request.codigoCliente,
                    detalle = request.detalle,
                    estado = request.estado,
                    Order_Details = new List<Order_Detail>(),
                    numero = request.numero,
                    total = request.total,
                    categoria = request.categoria,
                    equipo = request.equipo,
                    serie = request.serie,
                    color = request.color,
                    trajoAccesorio = request.trajoAccesorio,
                    accesorios = request.accesorios,
                    averia = request.averia,
                    fallaTecnica = request.fallaTecnica,
                    costoPieza = request.costoPieza,
                    repuesto = request.repuesto
                };
                _context.Repairs.Add(repair);
                foreach (Order_Detail item in request.Order_Details)
                {

                    //Checking if this product exist
                    Product product = await _context.Products.FindAsync(item.codigo_articulo);
                    if (product == null) { return Ok(new Response { IsSuccess = false, Message = "Producto no existe." }); }

                    //Checking if this product has enough qty in inventory
                    var inventory = _context.Inventories.Single(i => i.Id == item.idInventario);
                    int invent = inventory.Cantidad;
                    if (invent <= 0) { return Ok(new Response { IsSuccess = false, Message = "No hay esta cantidad disponible." }); }

                    //Inserting the item into order_detail table

                    //Inserting into order Details
                    repair.Order_Details.Add(new Order_Detail
                    {
                        Product = await _context.Products.FindAsync(item.codigo_articulo),
                        cantidad = item.cantidad,
                        descuento = item.descuento,
                        PrecioVenta = item.PrecioVenta,
                        itbis = item.itbis,
                        idInventario = item.idInventario,
                        referencia = item.referencia,
                        idFactura = item.idFactura

                    });




                    //Reduce the qty in inventory
                    var inv = _context.Inventories.Single(i => i.Id == item.idInventario);
                    inv.Cantidad -= item.cantidad;



                }
                _context.Repairs.Add(repair);

                bool response = false;
                string mes = "";
                var created = _context.SaveChanges();
                if (created > 0)
                {
                    response = true;
                    mes = "Se ha creado el documento exitosamente";
                }
                else
                {
                    response = false;
                    mes = "Hubo un problema al guardar la factura";

                }

                return Ok(new Response { IsSuccess = response, Message = mes });

            }
            catch (Exception exception)
            {
                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }

        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> EditRepair(int id)
        {
            try
            {
                var repairs = (from o in _context.Repairs.AsNoTracking()
                              from u in _context.Users.Where(x => x.Id == o.resgistradoPor)
                              from c in _context.Clients.Where(x => x.Id == o.codigoCliente).DefaultIfEmpty()
                              where o.Id == id
                              select new
                              {
                                  o.Id,
                                  o.FechaLocal,
                                  o.codigoCliente,
                                  o.repuesto,
                                  o.detalle,
                                  o.estado,
                                  o.resgistradoPor,
                                  o.serie,
                                  o.total,
                                  o.trajoAccesorio,
                                  o.accesorios,
                                  o.averia,
                                  o.categoria,
                                  o.color,
                                  o.costoPieza,
                                  o.equipo,
                                  o.numero,
                                  o.fallaTecnica,
                                  u.FullName,
                                  customerName = c.nombre,
                                  c.celular,
                                  c.correo,
                                  c.telefono
                              }).ToList();

                var Order_Details = (from d in _context.Order_Details.AsNoTracking()
                                     join p in _context.Products on d.codigo_articulo equals p.Id
                                     join i in _context.Inventories on d.idInventario equals i.Id
                                     where d.RepairId == id
                                     select new
                                     {
                                         d.cantidad,
                                         d.codigo_articulo,
                                         i.Ganancia,
                                         d.Id,
                                         d.idInventario,
                                         d.itbis,
                                         d.PorcientoDescuento,
                                         d.referencia,
                                         d.PrecioVenta,
                                         d.Total,
                                         d.totalCompra,
                                         d.totalVenta,
                                         p.nombre,
                                         p.garantia,
                                         PrecioV = i.PrecioVenta,
                                         i.PrecioCompra,
                                         Cant = i.Cantidad
                                     }).ToList();
                var results = new
                {
                    IsSuccess = true,
                    repairs,
                    Order_Details
                };
                return Ok(results);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return Ok(new Response { IsSuccess = false, Message = "Este factura ya existe" });
                }
                else
                {
                    return Ok(new Response { IsSuccess = false, Message = dbUpdateException.InnerException.Message });
                }
            }
            catch (Exception exception)
            {
                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }
        }

        [HttpPut]
        [ResponseType(typeof(Repair))]
        public async Task<IActionResult> EditRepair(Repair request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso." });
            }

            try
            {
                var repairToEdit = _context.Repairs.FirstOrDefault(o => o.Id == request.Id);
                if (repairToEdit != null)
                {
                    repairToEdit.User = await _context.Users.FindAsync(request.resgistradoPor);
                    repairToEdit.Cliente = await _context.Clients.FindAsync(request.codigoCliente);
                    repairToEdit.detalle = request.detalle;
                    repairToEdit.estado = request.estado;
                    repairToEdit.numero = request.numero;
                    repairToEdit.total = request.total;
                    repairToEdit.categoria = request.categoria;
                    repairToEdit.equipo = request.equipo;
                    repairToEdit.serie = request.serie;
                    repairToEdit.color = request.color;
                    repairToEdit.trajoAccesorio = request.trajoAccesorio;
                    repairToEdit.accesorios = request.accesorios;
                    repairToEdit.averia = request.averia;
                    repairToEdit.fallaTecnica = request.fallaTecnica;
                    repairToEdit.costoPieza = request.costoPieza;
                    repairToEdit.repuesto = request.repuesto;
                    repairToEdit.Order_Details = new List<Order_Detail>();
                }
                else
                {
                    return Ok(new Response { IsSuccess = false, Message = "Resgistro no existe" });
                }
                foreach (Order_Detail item in request.Order_Details.ToList())    
                {
                    //Checking if this product exist
                    Product product = await _context.Products.FindAsync(item.codigo_articulo);
                    if (product == null) { return Ok(new Response { IsSuccess = false, Message = "Producto no existe." }); }

                    //Checking if this product has enough qty in inventory
                    var inventory = _context.Inventories.Single(i => i.Id == item.idInventario);
                    int invent = inventory.Cantidad;
                    if (invent <= 0) { return Ok(new Response { IsSuccess = false, Message = "No hay esta cantidad disponible." }); }


                    if (item.Id == 0)
                    {

                        //Inserting into order Details
                        repairToEdit.Order_Details.Add(new Order_Detail
                        {
                            Product = await _context.Products.FindAsync(item.codigo_articulo),
                            cantidad = item.cantidad,
                            descuento = item.descuento,
                            PrecioVenta = item.PrecioVenta,
                            itbis = item.itbis,
                            idInventario = item.idInventario,
                            referencia = item.referencia,
                            idFactura = item.idFactura

                        });

                        //Reduce the qty in inventory
                        var inve = _context.Inventories.Single(i => i.Id == item.idInventario);
                        inve.Cantidad -= item.cantidad;
                    }
                    else if (item.Id > 0)
                    {
                        //Getting the qty for the previous order
                        var DetailForthisItem = _context.Order_Details.Single(o => o.Id == item.Id);
                        int cantForThePreViousOrder = DetailForthisItem.cantidad;
                        int oldProductId = DetailForthisItem.codigo_articulo;
                        string inventoryIdForTheProduct = DetailForthisItem.idInventario;


                        //updating the record with the new information
                        var orderDetailsToEdit = _context.Order_Details.Single(o => o.Id == item.Id);
                        if (orderDetailsToEdit != null)
                        {
                            orderDetailsToEdit.cantidad = item.cantidad;
                            orderDetailsToEdit.descuento = item.descuento;
                            orderDetailsToEdit.codigo_articulo = item.codigo_articulo;
                            orderDetailsToEdit.PrecioVenta = item.PrecioVenta;
                            orderDetailsToEdit.itbis = item.itbis;
                            orderDetailsToEdit.idInventario = item.idInventario;
                            orderDetailsToEdit.referencia = item.referencia;
                        }

                        if (item.codigo_articulo == oldProductId)
                        {
                            //Updateting the inventory with new qty inserted
                            var oldInv = _context.Inventories.Single(i => i.Id == item.idInventario);
                            oldInv.Cantidad += cantForThePreViousOrder;

                            var newInv = _context.Inventories.Single(i => i.Id == item.idInventario);
                            newInv.Cantidad -= item.cantidad;

                        }
                        else
                        {
                            var oldInv = _context.Inventories.Single(i => i.Id == inventoryIdForTheProduct);
                            oldInv.Cantidad += cantForThePreViousOrder;

                            var newInv = _context.Inventories.Single(i => i.Id == item.idInventario && i.ProductId == item.codigo_articulo);
                            newInv.Cantidad -= item.cantidad;

                        }
                    }
                }
                foreach (Order_Detail item in request.DeletedOrderItemIDs.ToList())
                {
                    //return Ok(new Response { IsSuccess = false, Message = "Nada de nada" });
                    var itemToDelete = _context.Order_Details.Single(o => o.Id == item.Id);
                    if (itemToDelete != null)
                    {
                        _context.Order_Details.Remove(itemToDelete);
                        var inv = _context.Inventories.Single(i => i.Id == item.idInventario);
                        inv.Cantidad += item.cantidad;
                    }
                }

                bool response = false;
                string mes = "";
                var created = _context.SaveChanges();
                if (created > 0)
                {
                    response = true;
                    mes = "Se ha creado el documento exitosamente";
                }
                else
                {
                    response = false;
                    mes = "Hubo un problema al actualizar el resgistro";

                }

                return Ok(new Response { IsSuccess = response, Message = mes });
            }
            catch (Exception exception)
            {
                return Ok(new Response { IsSuccess = false, Message = exception.InnerException.Message });
            }




        }

        [HttpPut]
        [Route("diagnostic")]
        public async Task<IActionResult> Diagnostic(Diagnostic request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso." });
            }
            try
            {
                Repair currentRepair = await _context.Repairs
                .Include(o => o.User)
                .Include(o => o.Cliente)
                .FirstOrDefaultAsync(o => o.Id == request.Id );
                if (currentRepair == null)
                {
                    return Ok(new Response { IsSuccess = false, Message = "Registro no existe." });
                }

                currentRepair.estado = request.estado;
                currentRepair.total = request.total;
                currentRepair.fallaTecnica = request.fallaTecnica;
                _context.Repairs.Update(currentRepair);
                await _context.SaveChangesAsync();
                return Ok(new Response { IsSuccess = true, Message = "Se ha registrado los cambios exitosamente." });


            }
            catch(Exception exception)
            {
                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }
        }

        [HttpPut]
        [Route("repaid")]
        public async Task<IActionResult> Repaid(RepairToOrder request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso." });
            }
            try
            {
                Repair currentRepair = await _context.Repairs
                .Include(o => o.User)
                .Include(o => o.Cliente)
                .FirstOrDefaultAsync(o => o.Id == request.Id);
                if (currentRepair == null)
                {
                    return Ok(new Response { IsSuccess = false, Message = "Registro no existe." });
                }

                currentRepair.estado = request.estado;
                currentRepair.detalle = request.detalle;
                currentRepair.repuesto = request.repuesto;
                _context.Repairs.Update(currentRepair);

                Order order = new Order
                {
                    fecha = DateTime.UtcNow,
                    User = user,
                    tipoDocumento = request.tipoDocumento,
                    Client = await _context.Clients.FindAsync(request.codigoCliente),
                    ncf = request.ncf,
                    referencia = request.referencia,
                    descuento = request.descuento,
                    detalle = request.detalleFactura,
                    estado = request.estadoFactura,
                    totaln = request.totaln,
                    itbistot = request.itbistot,
                    OrderNumber = request.OrderNumber,
                    metPago = request.metPago,
                    tipoFactura = request.tipoFactura,
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return Ok(new Response { IsSuccess = true, Message = "Se ha cambiado los cambios exitosamente." });


            }
            catch (Exception exception)
            {
                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }
        }
    }
}
