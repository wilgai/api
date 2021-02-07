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

namespace api.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ProductController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpGet]
       
         public async Task<IActionResult> GetProducts()
         {  
            try
            {

                var products = (from p in _context.Products
                                join b in _context.Brands on p.BrandId equals b.Id
                                join c in _context.Categories on p.CategoryId equals c.Id
                                join m in _context.Models on  p.ModelId equals m.Id
                                join s in _context.Providers on p.ProviderId equals s.Id
                                where p.Activo
                                orderby p.nombre
                              select new
                              {
                                  p.Id,
                                  p.ModelId,
                                  p.modificar_precio,
                                  p.nombre,
                                  nombre2=p.nombre,
                                  p.oferta,
                                  p.porciento_beneficio,
                                  p.porciento_minimo,
                                  p.ProviderId,
                                  p.referencia_interna,
                                  p.referencia_suplidor,
                                  p.tipo_impuesto,
                                  p.usuario_registro,
                                  p.acepta_descuento,
                                  p.Activo,
                                  p.BrandId,
                                  p.CategoryId,
                                  p.codigo,
                                  p.detalle,
                                  p.fecha_actualizacion,
                                  p.fecha_registro,
                                  p.foto,
                                  p.garantia,
                                  nombreSuplidor=s.nombre,
                                  nombreCategoria=c.nombre,
                                  nombreMarca=b.nombre,
                                  nombreModelo=m.nombre
                              }).ToList();
                var results = new
                {
                    IsSuccess = true,
                    products
                };
                return Ok(results);
            }
             
            catch (Exception exception)
            {

                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea!" });
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso para realizar esta operación.!" });
            }
            try
            {
                Product product = new Product
                {
                    nombre = request.nombre,
                    ProviderId = request.ProviderId,
                    usuario_registro = request.usuario_registro,
                    fecha_registro = DateTime.UtcNow,
                    fecha_actualizacion = DateTime.UtcNow,
                    tipo_impuesto = request.tipo_impuesto,
                    Activo = request.Activo,
                    foto = request.foto,
                    CategoryId = request.CategoryId,
                    referencia_interna = request.referencia_interna,
                    referencia_suplidor = request.referencia_suplidor,
                    oferta = request.oferta,
                    modificar_precio = request.modificar_precio,
                    acepta_descuento = request.acepta_descuento,
                    detalle = request.detalle,
                    BrandId = request.BrandId,
                    porciento_beneficio = request.porciento_beneficio,
                    porciento_minimo = request.porciento_minimo,
                    ModelId = request.ModelId,
                    codigo = request.codigo,
                    garantia = request.garantia,

                };
                _context.Products.Add(product);

                bool response = false;
                string mes = "";
                var created = _context.SaveChanges();
                if (created > 0)
                {
                    response = true;
                    mes = "Se ha guardo los cambios exitosamente";
                }
                else
                {
                    response = false;
                    mes = "Este nombre ya existe";

                }

                return Ok(new Response { IsSuccess = response, Message = mes });


            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return Ok(new Response { IsSuccess = false, Message = "Este nombre ya existe" });
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<IActionResult> EditProduct([FromBody] Product request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea!" });
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso para realizar esta operación.!" });
            }
            try
            {
                var itemToEdit = _context.Products.Single(o => o.Id == request.Id);
                if (itemToEdit != null)
                {
                    itemToEdit.nombre = request.nombre;
                    itemToEdit.ProviderId = request.ProviderId;
                    itemToEdit.usuario_registro = request.usuario_registro;
                    itemToEdit.fecha_actualizacion = DateTime.UtcNow;
                    itemToEdit.tipo_impuesto = request.tipo_impuesto;
                    itemToEdit.Activo = request.Activo;
                    itemToEdit.foto = request.foto;
                    itemToEdit.CategoryId = request.CategoryId;
                    itemToEdit.referencia_interna = request.referencia_interna;
                    itemToEdit.referencia_suplidor = request.referencia_suplidor;
                    itemToEdit.oferta = request.oferta;
                    itemToEdit.modificar_precio = request.modificar_precio;
                    itemToEdit.acepta_descuento = request.acepta_descuento;
                    itemToEdit.detalle = request.detalle;
                    itemToEdit.BrandId = request.BrandId;
                    itemToEdit.porciento_beneficio = request.porciento_beneficio;
                    itemToEdit.porciento_minimo = request.porciento_minimo;
                    itemToEdit.ModelId = request.ModelId;
                    itemToEdit.codigo = request.codigo;
                    itemToEdit.garantia = request.garantia;

                }



                _context.SaveChanges();

                return Ok(new Response { IsSuccess = true, Message = "Se ha guardo los cambios exitosamente" });


            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return Ok(new Response { IsSuccess = false, Message = "Este nombre  ya existe" });
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


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("deleteProduct")]
        public async Task<IActionResult> DeleteProvider([FromBody] Product request)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso para realizar esta operación.!" });
            }

            try
            {
                var itemToDelete = _context.Products.Single(o => o.Id == request.Id);
                if (itemToDelete != null)
                {
                    _context.Products.Remove(itemToDelete);
                }
                else
                {
                    return Ok(new Response { IsSuccess = false, Message = "Registro no existe." });
                }

                _context.SaveChanges();

                return Ok(new Response { IsSuccess = true, Message = "Se ha guardo los cambios exitosamente" });


            }

            catch (Exception exception)
            {

                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }

        }

    }
}
