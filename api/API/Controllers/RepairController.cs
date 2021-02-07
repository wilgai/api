using api.Data;
using api.Entities;
using api.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        [HttpPost]
        public async Task<IActionResult> PostRepair([FromBody] Repair request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

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
            if (request.Id == 0)
            {
                _context.Repairs.Add(repair);
            }
            else
            {
                _context.Entry(repair).State = EntityState.Modified;
            }


            foreach (Order_Detail item in request.Order_Details)
            {
                Product product = await _context.Products.FindAsync(item.codigo_articulo);
                if (product == null) { return NotFound("Error002"); }

                //Checking if this product has enough qty in inventory
                var inventory = _context.Inventories.Single(inv => inv.Id == item.idInventario);
                int invent = inventory.Cantidad;
                if (invent <= 0) { return NotFound("Error002+"); }

                if (item.Id == 0)
                {
                    //Inserting the item into order_detail table
                    _context.Order_Details.Add(item);
                    //Reduce the qty in inventory
                    var inv = _context.Inventories.Single(i => i.Id == item.idInventario);
                    inv.Cantidad -= item.cantidad;
                    _context.SaveChanges();
                }
                else
                {
                    //Getting the qty for the previous order
                    var DetailForthisItem = _context.Order_Details.Single(o => o.Id == item.Id);
                    int cantForThePreViousOrder = DetailForthisItem.cantidad;

                    //updating the record with the new information
                    _context.Entry(item).State = EntityState.Modified;

                    //Updateting the inventory with new qty inserted
                    var inv = _context.Inventories.Single(i => i.Id == item.idInventario);
                    inv.Cantidad -= (item.cantidad + cantForThePreViousOrder) - cantForThePreViousOrder;
                    _context.SaveChanges();
                }


            }
            foreach (Order_Detail item in request.DeletedOrderItemIDs)
            {
                var itemToDelete = _context.Order_Details.Single(o => o.Id == item.Id);
                if (itemToDelete != null)
                {
                    _context.Order_Details.Remove(itemToDelete);
                    var inv = _context.Inventories.Single(i => i.Id == item.idInventario);
                    inv.Cantidad += item.cantidad;
                    _context.SaveChanges();
                }

            }

            _context.SaveChanges();
            return Ok();
        }
    }
}
