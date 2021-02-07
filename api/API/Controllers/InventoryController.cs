using api.Data;
using api.Helper;
using api.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace api.API.Controllers
{

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        public InventoryController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task<IActionResult> GetInventories()
        {


            try
            {
                var inventories = (from i in _context.Inventories.AsNoTracking()
                                   from p in _context.Products.Where(x => x.Id == i.ProductId)
                                   .OrderByDescending(inv => Convert.ToDateTime(i.Fecha)).Take(10)
                                   select new
                                   {
                                       p.Id,
                                       p.nombre,
                                       nombre2 = p.nombre,
                                       p.ProviderId,
                                       p.usuario_registro,
                                       p.fecha_registro,
                                       p.fecha_actualizacion,
                                       p.tipo_impuesto,
                                       p.Activo,
                                       p.CategoryId,
                                       p.referencia_interna,
                                       p.referencia_suplidor,
                                       p.foto,
                                       p.oferta,
                                       p.modificar_precio,
                                       p.acepta_descuento,
                                       p.detalle,
                                       p.BrandId,
                                       p.porciento_beneficio,
                                       p.porciento_minimo,
                                       p.ModelId,
                                       p.codigo,
                                       idInventario = i.Id,
                                       i.ProductId,
                                       i.PrecioCompra,
                                       i.Ganancia,
                                       i.PrecioVenta,
                                       i.Itbis,
                                       i.Cantidad,
                                       i.Fecha,
                                   }).ToList();

                var results = new
                {
                    IsSuccess = true,
                    inventories
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
