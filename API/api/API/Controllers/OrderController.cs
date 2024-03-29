﻿using api.Data;
using api.Entities;
using api.Helper;
using api.Request;
using api.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                 .Where(o => o.tipoDocumento == "Factura" || o.tipoDocumento == "Reparacion")
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
        [Route("GetTodaySOrder")]
        public async Task<IActionResult> GetTodaySOrder()
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
                DateTime date = DateTime.Now;
                List<Order> orders = await _context.Orders
                 .Include(o => o.User)
                 .Include(c => c.Client)
                 .Where(o => o.tipoDocumento == "Factura" || o.tipoDocumento == "Reparacion" && o.fecha == date)
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
                 .Where(o => o.tipoDocumento == "Compra")
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
                    fecha = DateTime.Today,
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
                            descuento = item.descuento,
                            PrecioVenta = item.PrecioVenta,
                            itbis = item.itbis,
                            idInventario = item.idInventario,
                            referencia = item.referencia,
                            idFactura = request.OrderNumber

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
                            descuento = item.descuento,
                            PrecioVenta = item.PrecioVenta,
                            itbis = item.itbis,
                            idInventario = item.idInventario,
                            referencia = item.referencia,
                            idFactura = request.OrderNumber
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
                            OrderNumber = request.OrderNumber
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

        [HttpGet("{id}")]
        public async Task<IActionResult> EditOrder(string id)
        {
            try
            {


                var orders = (from o in _context.Orders.AsNoTracking()
                              from u in _context.Users.Where(x => x.Id == o.usuario_registro)
                              from c in _context.Clients.Where(x => x.Id == o.codigoCliente).DefaultIfEmpty()
                              from p in _context.Providers.Where(x => x.Id == o.suplidor).DefaultIfEmpty()
                              from r in _context.Repairs.Where(x => o.OrderNumber == x.numero).DefaultIfEmpty()
                              where o.OrderNumber == id
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
                                  c.celular,
                                  providerName = p.nombre,
                                  providerPhone = p.telefono,
                                  providerAdress = p.direccion,
                                  Rep = r.numero == null ? null : r,



                              }).ToList();

                var Order_Details = (from d in _context.Order_Details.AsNoTracking()
                                     from p in _context.Products.Where(x => x.Id == d.codigo_articulo).DefaultIfEmpty()
                                     from i in _context.Inventories.Where(x => x.Id == d.idInventario).DefaultIfEmpty()
                                     where d.idFactura == id
                                     select new
                                     {
                                         d.cantidad,
                                         d.descuento,
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
                var Payments = (from p in _context.Payments
                                where p.orderID == id
                                group p by p.orderID into g
                                select new { Total = g.Sum(x => x.TotalPaid) }).ToList();
                decimal TotalPaid = 0;

                foreach (var x in Payments)
                {
                    TotalPaid = x.Total;
                }
                var results = new
                {
                    IsSuccess = true,
                    orders,
                    Order_Details,
                    TotalPaid


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

        [HttpGet]
        [Route("GetRevenueByMonth")]
        public async Task<IActionResult> GetRevenueByMonth()
        {
            try
            {
               


                var orders = (from i in _context.Inventories.AsNoTracking()
                              from d in _context.Order_Details.Where(x => x.idFactura == i.OrderNumber).DefaultIfEmpty()
                              from o in _context.Orders.Where(x => x.OrderNumber == i.OrderNumber).DefaultIfEmpty()
                              where o.fecha.Year == o.fecha.Year && o.estado == "Entregado"
                              select new
                              {

                                  month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(o.fecha.Month),
                                  revenue = (d.cantidad - i.Cantidad) * i.PrecioVenta - i.PrecioCompra * (d.cantidad - i.Cantidad)
                              }).ToList();
                var results = new
                {
                    IsSuccess = true,
                    orders,
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

        [HttpGet]
        [Route("GetMonthSAles")]
        public async Task<IActionResult> GetMonthSAles()
        {
            try
            {
                


                var orders = (from i in _context.Inventories.AsNoTracking()
                              from d in _context.Order_Details.Where(x => x.idFactura == i.OrderNumber).DefaultIfEmpty()
                              from o in _context.Orders.Where(x => x.OrderNumber == i.OrderNumber).DefaultIfEmpty()
                              where o.fecha.Year == o.fecha.Year && o.estado == "Entregado"
                              select new
                              {
                                  month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(o.fecha.Month),
                                  total = (d.cantidad - i.Cantidad) * i.PrecioVenta

                              }).ToList();
                var results = new
                {
                    IsSuccess = true,
                    orders,
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

                    orderToEdit.User = user;
                    orderToEdit.tipoDocumento = request.tipoDocumento;
                    orderToEdit.Client = await _context.Clients.FindAsync(request.codigoCliente);
                    orderToEdit.ncf = request.ncf;
                    orderToEdit.referencia = request.referencia;
                    orderToEdit.descuento = request.descuento;
                    orderToEdit.detalle = request.detalle;
                    orderToEdit.estado = "Pendiente";
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
                                descuento = item.descuento,
                                PrecioVenta = item.PrecioVenta,
                                itbis = item.itbis,
                                idInventario = item.idInventario,
                                referencia = item.referencia,
                                idFactura = request.OrderNumber

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
                                idFactura = request.OrderNumber

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
                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }




        }

        [HttpPut]
        [Route("cancelOrder")]
        public async Task<IActionResult> CancelOrder(Order request)
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
                double days = 0;

                var orderToEdit = _context.Orders.FirstOrDefault(o => o.Id == request.Id);
                if (orderToEdit != null)
                {
                    DateTime startDate = orderToEdit.fecha;
                    DateTime endDate = DateTime.Now;
                    days = (endDate - startDate).TotalDays;

                    orderToEdit.estado = "Cancelado";
                }
                else
                {
                    return Ok(new Response { IsSuccess = false, Message = "Resgistro no existe" });
                }

                foreach (Order_Detail item in request.Order_Details.ToList())
                {
                    bool expired = false;
                    if (item.garantia == 0 || item.garantia <= days)
                    {
                        expired = true;
                    }
                    if (request.tipoDocumento == "Factura")
                    {
                        //Checking if this product exist
                        Product product = await _context.Products.FindAsync(item.codigo_articulo);
                        if (product == null) { return Ok(new Response { IsSuccess = false, Message = "Producto no existe." }); }
                        if (expired && request.estado != "Pendiente")
                        {
                            return Ok(new Response { IsSuccess = false, Message = "Se ha vencido la garantia de esta factura." });
                        }

                        if (item.Id > 0)
                        {
                            //Updateting the inventory with new qty inserted
                            var oldInv = _context.Inventories.Single(i => i.Id == item.idInventario);
                            oldInv.Cantidad += item.cantidad;
                        }
                    }
                    else
                    {
                        if (request.estado != "Pendiente")
                        {
                            return Ok(new Response { IsSuccess = false, Message = "Se ha podido cancel esta factura." });
                        }

                        if (item.Id > 0)
                        {
                            //Updateting the inventory with new qty inserted
                            var oldInv = _context.Inventories.Single(i => i.Id == item.idInventario);
                            oldInv.Cantidad -= item.cantidad;
                        }

                    }
                }
                bool response = false;
                string mes = "";
                var created = _context.SaveChanges();
                if (created > 0)
                {
                    response = true;
                    mes = "La factura ha sido cancelado.";
                }
                else
                {
                    response = false;
                    mes = "Hubo un problema al cancelar la factura.";

                }

                return Ok(new Response { IsSuccess = response, Message = mes });
            }
            catch (Exception exception)
            {
                return Ok(new Response { IsSuccess = false, Message = exception.InnerException.Message });
            }

        }


        [HttpPut]
        [Route("checkout")]
        public async Task<IActionResult> EditOrder(CheckoutRequest request)
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
                Payment payment = new Payment
                {
                    TotalPaid = request.TotalPaid,
                    orderID = request.orderId,
                    Date = DateTime.UtcNow,
                    Reference = request.Reference,
                    BillPaidWith = request.BillPaidWith,
                    Change = request.Change
                };
                _context.Payments.Add(payment);

                bool response = false;
                string mes = "";
                var created = _context.SaveChanges();
                if (created > 0)
                {


                    decimal TotalPaid = 0;
                    decimal TotalPaidBefore = 0;
                    var Payments = (from p in _context.Payments
                                    where p.orderID == request.orderId
                                    group p by p.orderID into g
                                    select new { Total = g.Sum(x => x.TotalPaid) });
                    foreach (var x in Payments)
                    {
                        TotalPaid = x.Total;
                    }
                    var order = _context.Orders.FirstOrDefault(o => o.OrderNumber == request.orderId);
                    if (order != null)
                    {
                        TotalPaidBefore = order.totaln;
                    }
                    else
                    {
                        return Ok(new Response { IsSuccess = false, Message = "Resgistro no existe" });
                    }

                    if (TotalPaid == TotalPaidBefore)
                    {
                        var orderToEdit = _context.Orders.FirstOrDefault(o => o.OrderNumber == request.orderId);
                        if (orderToEdit != null)
                        {
                            orderToEdit.estado = request.estado;
                        }
                        else
                        {
                            return Ok(new Response { IsSuccess = false, Message = "Resgistro no existe" });
                        }
                        created = _context.SaveChanges();
                        if (created > 0)
                        {
                            response = true;
                            mes = "Gracias por su compra!!!";
                        }
                        else
                        {
                            response = false;
                            mes = "Hubo un problema al procesar el pago.";
                        }

                    }
                    else if (TotalPaid > TotalPaidBefore)
                    {
                        var orderToEdit = _context.Orders.FirstOrDefault(o => o.OrderNumber == request.orderId);
                        if (orderToEdit != null)
                        {
                            orderToEdit.estado = request.estado;
                        }
                        else
                        {
                            return Ok(new Response { IsSuccess = false, Message = "Resgistro no existe" });
                        }
                        created = _context.SaveChanges();
                        if (created > 0)
                        {
                            response = true;
                            mes = "El pago de esta factura esta completo";
                        }
                        else
                        {
                            response = false;
                            mes = "Hubo un problema al procesar el pago.";
                        }

                    }
                    else if (TotalPaid < TotalPaidBefore)
                    {



                        response = true;
                        mes = "Se ha registrado el avance de pago";





                    }
                    return Ok(new Response { IsSuccess = response, Message = mes });
                }
                else
                {
                    return Ok(new Response { IsSuccess = false, Message = "No se pudo procesar el pago." });
                }
            }
            catch (Exception exception)
            {
                return Ok(new Response { IsSuccess = false, Message = exception.InnerException.Message });
            }
        }

        [HttpGet]
        [Route("GetMiniCardData")]
        public async Task<IActionResult> GetMiniCardData()
        {
            DateTime fech = DateTime.Now;

            try
            {

                var inventories = (from i in _context.Inventories.AsNoTracking()
                                   from d in _context.Order_Details.Where(x => x.idFactura == i.OrderNumber).DefaultIfEmpty()
                                   from o in _context.Orders.Where(x => x.OrderNumber == i.OrderNumber).DefaultIfEmpty()
                                   where o.estado == "Entregado" && o.fecha.Month== fech.Month
                                   select new
                                   {
                                       cantidadVendido = d.cantidad - i.Cantidad,
                                       totalComprado = d.cantidad * i.PrecioCompra,
                                       totalVendido = (d.cantidad - i.Cantidad) * i.PrecioVenta,
                                       porcientoVendido = ((d.cantidad - i.Cantidad) * 100) / d.cantidad,
                                       totalGanacia = (d.cantidad - i.Cantidad) * i.PrecioVenta - i.PrecioCompra * (d.cantidad - i.Cantidad)
                                   }).ToList();
                var todaySales = (from o in _context.Orders
                                  where (o.estado== "Entregado" && o.tipoDocumento=="Factura") && o.fecha.Date==DateTime.Today
                                  group o by o.fecha into g
                                  select new { Total = g.Sum(x => x.totaln) });
              
                decimal TotalSales = 0;
                decimal TotalPurcharses = 0;
                decimal TotalQuantity = 0;
                decimal TotalRevenue = 0;
                decimal TotalTodaySales = 0;


                decimal PorcentageSales = 0;
               decimal  porcentageRevenue = 0;
                decimal porcentaItemSales = 0;
                decimal TotaysPorcentaSales = 0;
                decimal TodaysItemSaleQty = 0;
                foreach (var item in inventories)
                {
                    TotalSales += item.totalVendido;
                    TotalPurcharses += item.totalComprado;
                    TotalQuantity += item.cantidadVendido;
                    TotalRevenue  += item.totalGanacia;
                }
                foreach (var item in todaySales)
                {
                    TotalTodaySales += item.Total;
                    
                }
                PorcentageSales = (TotalSales * 100) / 100000;
                porcentageRevenue= (TotalRevenue * 100) / 50000;
                porcentaItemSales= (TotalQuantity * 100) / 100;
                TotaysPorcentaSales = (TotalTodaySales * 100) / 20000;


                var nimiCards = new List<MiniCardInfo>()
                {
                    new MiniCardInfo() { icon = "account_balance_wallet", title = "Total Sales", value =TotalSales, color = "primary", isIncrease = porcentaItemSales >=50? true:false, isCurrency = true, duration = "Since last month", percentValue = porcentaItemSales },
                    new MiniCardInfo() { icon = "request_quote", title = "Total Revenue", value =TotalRevenue, color = "accent", isIncrease = porcentageRevenue >=50? true:false, isCurrency = true, duration = "Since last month", percentValue = porcentageRevenue },
                     new MiniCardInfo() { icon = "shopping_cart", title = "This Month Sales Quantity", value =TotalQuantity, color = "", isIncrease = porcentaItemSales >=50? true:false, isCurrency = false, duration = "Since last month", percentValue = porcentaItemSales },
                      new MiniCardInfo() { icon = "request_quote", title = "Today's Sales", value =TotalTodaySales, color = "warn", isIncrease = TotaysPorcentaSales >=50? true:false, isCurrency = true, duration = "Since Yesterday", percentValue = TotaysPorcentaSales },

                };


                var results = new
                {
                    IsSuccess = true,
                    nimiCards,
                  



                };
                return Ok(results);
            }
            catch (Exception exception)
            {

                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }
        }




    }
}




