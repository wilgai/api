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
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrderController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
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
                List<Order> orders = await _context.Orders
                 .Include(o => o.User)
                 .Include(c => c.Client)
                 .Where(o => o.tipoDocumento == "Factura")
                 .OrderByDescending(o => o.fecha)
                 .ToListAsync();
                var results = new
                {
                    IsSuccess = true,
                    orders
                };
                return Ok(results);
            }
            catch (Exception exception)
            {

                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }

        }
        
        [HttpGet]
        [Route("getPurchases")]
        public async Task<IActionResult> GetPurchases()
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
                List<Order> orders = await _context.Orders
                 .Include(o => o.User)
                 .Include(p => p.Provider)
                 .Where(o=>o.tipoDocumento=="Compra")
                 .OrderByDescending(o => o.fecha)
                 .ToListAsync();
                var results = new
                {
                    IsSuccess = true,
                    orders
                };
                return Ok(results);
            }
            catch (Exception exception)
            {

                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }

        }
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] Order request)
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
                /* Order orderExists = await _context.Orders.Where(o => o.OrderNumber == request.OrderNumber)
                     .FirstAsync();

                 if (orderExists != null) { return Ok(new Response { IsSuccess = false, Message = $"Factura {request.OrderNumber}. ya existe." }); }*/

                Order order = new Order
                {
                    fecha = DateTime.UtcNow,
                    User = user,
                    tipoDocumento = request.tipoDocumento,
                    Client = await _context.Clients.FindAsync(request.codigoCliente),
                    ncf = request.ncf,
                    referencia = request.referencia,
                    descuento = request.descuento,
                    detalle = request.detalle,
                    estado = request.estado,
                    totaln = request.totaln,
                    itbistot = request.itbistot,
                    OrderNumber = request.OrderNumber,
                    metPago = request.metPago,
                    tipoFactura = request.tipoFactura,
                    Provider = await _context.Providers.FindAsync(request.suplidor),
                    Order_Details = new List<Order_Detail>(),

                };
                _context.Orders.Add(order);
                foreach (Order_Detail item in request.Order_Details)
                {
                    if (request.tipoDocumento == "Factura")
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
                        order.Order_Details.Add(new Order_Detail
                        {
                            Product = await _context.Products.FindAsync(item.codigo_articulo),
                            cantidad = item.cantidad,
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
                    else if (request.tipoDocumento == "Compra")
                    {
                        //Checking if this product exist
                        Product product = await _context.Products.FindAsync(item.codigo_articulo);
                        if (product == null) { return Ok(new Response { IsSuccess = false, Message = "Producto no existe." }); }
                        item.idInventario = Guid.NewGuid().ToString();
                        //Inserting into order Details
                        order.Order_Details.Add(new Order_Detail
                        {
                            Product = await _context.Products.FindAsync(item.codigo_articulo),
                            cantidad = item.cantidad,
                            PrecioVenta = item.PrecioVenta,
                            itbis = item.itbis,
                            idInventario = item.idInventario,
                            referencia = item.referencia,
                            idFactura = item.idFactura
                        });
                        //Inserting into inventory
                        Inventory inventory = new Inventory
                        {
                            Id = item.idInventario,
                            Producto = await _context.Products.FindAsync(item.codigo_articulo),
                            PrecioCompra = item.PrecioCompra,
                            Ganancia = item.Ganancia,
                            PrecioVenta = item.PrecioVenta,
                            PorcientoDescuento = item.PorcientoDescuento,
                            Cantidad = item.cantidad,
                            Fecha = DateTime.UtcNow,
                            OrderNumber = item.idFactura
                        };
                        _context.Inventories.Add(inventory);


                    }
                }
                _context.Orders.Add(order);

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
        public async Task<IActionResult> EditOrder(int id)
        {
            try
            {
                var orders = (from o in _context.Orders.AsNoTracking()
                              from u in _context.Users.Where(x => x.Id == o.usuario_registro)
                              from c in _context.Clients.Where(x => x.Id == o.codigoCliente).DefaultIfEmpty()
                              from p in _context.Providers.Where(x => x.Id == o.suplidor).DefaultIfEmpty()
                              where o.Id == id
                              select new
                              {
                                  o.Id,
                                  o.FechaLocal,
                                  o.codigoCliente,
                                  o.descuento,
                                  o.detalle,
                                  o.estado,
                                  o.fecha,
                                  o.itbistot,
                                  o.metPago,
                                  o.ncf,
                                  o.OrderNumber,
                                  o.suplidor,
                                  o.referencia,
                                  o.tipoDocumento,
                                  o.tipoFactura,
                                  o.totaln,
                                  o.usuario_registro,
                                  o.ValorCpmpra,
                                  o.ValorVenta,
                                  u.FullName,
                                  customerName = c.nombre,
                                  providerName = p.nombre
                              }).ToList();

                var Order_Details = (from d in _context.Order_Details.AsNoTracking()
                                     join p in _context.Products on d.codigo_articulo equals p.Id
                                     join i in _context.Inventories on d.idInventario equals i.Id
                                     where d.OrderID == id
                                     select new
                                     {
                                         d.cantidad,
                                         d.codigo_articulo,
                                         i.Ganancia,
                                         d.Id,
                                         d.idInventario,
                                         d.itbis,
                                         d.PorcientoDescuento,

                                         d.PrecioVenta,
                                         d.referencia,
                                         d.Total,
                                         d.totalCompra,
                                         d.totalVenta,
                                         p.nombre,
                                         PrecioV = i.PrecioVenta,
                                         i.PrecioCompra,
                                         Cant = i.Cantidad
                                     }).ToList();



                var results = new
                {
                    IsSuccess = true,
                    orders,
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
        [ResponseType(typeof(Order))]
        public async Task<IActionResult> EditOrder(Order request)
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
                var orderToEdit = _context.Orders.FirstOrDefault(o => o.Id == request.Id);
                if (orderToEdit != null)
                {
                    orderToEdit.fecha = DateTime.UtcNow;
                    orderToEdit.User = user;
                    orderToEdit.tipoDocumento = request.tipoDocumento;
                    orderToEdit.Client = await _context.Clients.FindAsync(request.codigoCliente);
                    orderToEdit.ncf = request.ncf;
                    orderToEdit.referencia = request.referencia;
                    orderToEdit.descuento = request.descuento;
                    orderToEdit.detalle = request.detalle;
                    orderToEdit.estado = request.estado;
                    orderToEdit.totaln = request.totaln;
                    orderToEdit.itbistot = request.itbistot;
                    orderToEdit.OrderNumber = request.OrderNumber;
                    orderToEdit.metPago = request.metPago;
                    orderToEdit.tipoFactura = request.tipoFactura;
                    orderToEdit.suplidor = request.suplidor;
                    orderToEdit.Provider = await _context.Providers.FindAsync(request.suplidor);


                }
                else
                {
                    return Ok(new Response { IsSuccess = false, Message = "Resgistro no existe" });
                }

                foreach (Order_Detail item in request.Order_Details.ToList())
                {
                    if (request.tipoDocumento == "Factura")
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
                            orderToEdit.Order_Details.Add(new Order_Detail
                            {
                                Product = await _context.Products.FindAsync(item.codigo_articulo),
                                cantidad = item.cantidad,
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
                            

                            //updating the record with the new information
                            var orderDetailsToEdit = _context.Order_Details.Single(o => o.Id == item.Id);
                            if (orderDetailsToEdit != null)
                            {
                                orderDetailsToEdit.cantidad = item.cantidad;
                                orderDetailsToEdit.codigo_articulo = item.codigo_articulo;
                                orderDetailsToEdit.PrecioVenta = item.PrecioVenta;
                                orderDetailsToEdit.itbis = item.itbis;
                                orderDetailsToEdit.idInventario = item.idInventario;
                                orderDetailsToEdit.referencia = item.referencia;
                            }
                            //Updateting the inventory with new qty inserted

                            var oldInv = _context.Inventories.Single(i => i.Id == item.idInventario);
                            oldInv.Cantidad += cantForThePreViousOrder;

                            var newInv = _context.Inventories.Single(i => i.Id == item.idInventario);
                            newInv.Cantidad -= item.cantidad;

                        }
                    }
                    else if (request.tipoDocumento == "Compra")
                    {
                        //Checking if this product exist
                        Product product = await _context.Products.FindAsync(item.codigo_articulo);
                        if (product == null) { return Ok(new Response { IsSuccess = false, Message = "Producto no existe." }); }
                        string idInv = Guid.NewGuid().ToString();
                        if (item.Id == 0)
                        {

                            //Inserting into order Details
                            orderToEdit.Order_Details.Add(new Order_Detail
                            {
                                Product = await _context.Products.FindAsync(item.codigo_articulo),
                                cantidad = item.cantidad,
                                PrecioVenta = item.PrecioVenta,
                                itbis = item.itbis,
                                idInventario = idInv,
                                referencia = item.referencia,
                                idFactura = item.idFactura

                            });

                            //Inserting into inventory
                            Inventory invent = new Inventory
                            {
                                Id = idInv,
                                Producto = await _context.Products.FindAsync(item.codigo_articulo),
                                PrecioCompra = item.PrecioCompra,
                                Ganancia = item.Ganancia,
                                PrecioVenta = item.PrecioVenta,
                                PorcientoDescuento = item.PorcientoDescuento,
                                Cantidad = item.cantidad,
                                Fecha = DateTime.UtcNow
                            };
                            _context.Inventories.Add(invent);

                        }
                        else if (item.Id > 0)
                        {
                            //Getting the qty for the previous order
                            var DetailForthisItem = _context.Order_Details.FirstOrDefault(o => o.Id == item.Id);
                            int cantForThePreViousOrder = DetailForthisItem.cantidad;

                            var orderDetailsToEdit = _context.Order_Details.FirstOrDefault(o => o.Id == item.Id);
                            if (orderDetailsToEdit == null)
                            {
                                return Ok(new Response { IsSuccess = false, Message = "No se ha encontrado este item." });
                            }
                            else if (orderDetailsToEdit != null)
                            {
                                orderDetailsToEdit.cantidad = item.cantidad;
                                orderDetailsToEdit.codigo_articulo = item.codigo_articulo;
                                orderDetailsToEdit.PrecioVenta = item.PrecioVenta;
                                orderDetailsToEdit.itbis = item.itbis;
                                orderDetailsToEdit.idInventario = item.idInventario;
                                orderDetailsToEdit.referencia = item.referencia;

                            }
                            var inventoryToEdit = _context.Inventories.FirstOrDefault(i => i.Id == item.idInventario);
                            if (inventoryToEdit == null)
                            {
                                return Ok(new Response { IsSuccess = false, Message = "Inventario no existe." });
                            }
                            else if (inventoryToEdit != null)
                            {
                                inventoryToEdit.Producto = await _context.Products.FindAsync(item.codigo_articulo);
                                inventoryToEdit.PrecioCompra = item.PrecioCompra;
                                inventoryToEdit.Ganancia = item.Ganancia;
                                inventoryToEdit.PrecioVenta = item.PrecioVenta;
                                inventoryToEdit.PorcientoDescuento = item.PorcientoDescuento;
                                inventoryToEdit.Cantidad = item.cantidad;



                            }
                            //Updateting the inventory with new qty inserted
                            var inv = _context.Inventories.AsNoTracking().FirstOrDefault(i => i.Id == item.idInventario);
                            inv.Cantidad -= (item.cantidad + cantForThePreViousOrder) - cantForThePreViousOrder;

                        }

                    }

                }
                foreach (Order_Detail item in request.DeletedOrderItemIDs.ToList())
                {
                    //return Ok(new Response { IsSuccess = false, Message = "Nada de nada" });
                    if (request.tipoDocumento == "Factura")
                    {
                        var itemToDelete = _context.Order_Details.Single(o => o.Id == item.Id);
                        if (itemToDelete != null)
                        {
                            _context.Order_Details.Remove(itemToDelete);
                            var inv = _context.Inventories.Single(i => i.Id == item.idInventario);
                            inv.Cantidad += item.cantidad;
                        }

                    }
                    else if (request.tipoDocumento == "Compra")
                    {
                        var orderToDelete = _context.Order_Details.Single(o => o.Id == item.Id);
                        if (orderToDelete != null)
                        {
                            _context.Order_Details.Remove(orderToDelete);
                            var inv = _context.Inventories.Where(i => i.Id == item.idInventario).First();
                            _context.Inventories.Remove(inv);
                           
                        }
                      
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
                    mes = "Hubo un problema al guardar la factura";

                }

                return Ok(new Response { IsSuccess = response, Message = mes });




            }
            catch (Exception exception)
            {
                return Ok(new Response { IsSuccess = false, Message = exception.InnerException.Message });
            }




        }






    }
}




