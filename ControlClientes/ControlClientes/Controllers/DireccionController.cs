using api.Responses;
using ControlClientes.Data;
using ControlClientes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlClientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DireccionController : Controller
    {
        private readonly DataContext _context;


        public DireccionController(DataContext context)
        {
            _context = context;
        }
        [HttpPut]
        public async Task<IActionResult> ActualizarDireccion(Direccion request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }
            try
            {
                var direccionParaEditar = _context.Direcciones.FirstOrDefault(o => o.Id == request.Id);
                string[] separadas = request.Provicia.Split('|');
                string prov = separadas[1];
                if (direccionParaEditar != null)
                {
                    direccionParaEditar.LineaDeDireccion1 = request.LineaDeDireccion1;
                    direccionParaEditar.LineaDeDireccion2 = request.LineaDeDireccion2;
                    direccionParaEditar.Pais = request.Pais;
                    direccionParaEditar.Provicia = prov;
                    direccionParaEditar.Municipio = request.Municipio;
                    direccionParaEditar.CodigoPostal = request.CodigoPostal;
                }
                else
                {
                    return Ok(new Response { IsSuccess = false, Message = "Resgistro no existe" });
                }

                bool response = false;
                string mes = "";
                var created = await _context.SaveChangesAsync();
                if (created > 0)
                {
                    response = true;
                    mes = "Se ha actualizado el registro exitosamente";
                }
                else
                {
                    response = false;
                    mes = "Hubo un problema al guardar el registro";

                }

                return Ok(new Response { IsSuccess = response, Message = mes });

            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return Ok(new Response { IsSuccess = false, Message = "Resgistro ya existe" });
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
        [Route("eliminarDireccion")]
        public async Task<IActionResult> EliminarCliente(Direccion request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }

            try
            {
                var direccParaEliminar = _context.Direcciones.FirstOrDefault(o => o.Id == request.Id);
                if (direccParaEliminar != null)
                {

                    _context.Direcciones.Remove(direccParaEliminar);
                }

                else
                {
                    return Ok(new Response { IsSuccess = false, Message = "Resgistro no existe" });
                }
                bool response = false;
                string mes = "";
                var created = await _context.SaveChangesAsync();
                if (created > 0)
                {
                    response = true;
                    mes = "La direccion ha sido eliminado.";
                }
                else
                {
                    response = false;
                    mes = "Hubo un problema al eliminar la direccion.";

                }

                return Ok(new Response { IsSuccess = response, Message = mes });
            }
            catch (Exception exception)
            {
                return Ok(new Response { IsSuccess = false, Message = exception.InnerException.Message });
            }

        }
        [HttpPost]
        public async Task<IActionResult> GuardarDireccion([FromBody] Direccion request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }
            try
            {
             
                Direccion direccion = new Direccion
                {
                   

                    Cliente = await _context.Clientes.FindAsync(request.codigoCliente),
                    LineaDeDireccion1 = request.LineaDeDireccion1,
                    LineaDeDireccion2 = request.LineaDeDireccion2,
                    Pais = request.Pais,
                    Provicia = request.Provicia,
                    Municipio = request.Municipio,
                    CodigoPostal = request.CodigoPostal,
                };
                _context.Direcciones.Add(direccion);
                bool response = false;
                string mes = "";
                var created = await _context.SaveChangesAsync();
                if (created > 0)
                {
                    response = true;
                    mes = "Se ha creado la dirección exitosamente";
                }
                else
                {
                    response = false;
                    mes = "Hubo un problema al guardar la dirección";

                }

                return Ok(new Response { IsSuccess = response, Message = mes });

            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return Ok(new Response { IsSuccess = false, Message = "Resgistro ya existe" });
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
        public async Task<IActionResult> ObtenerDirecciones()
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }
            try
            {
                List<Direccion> direcciones = await _context.Direcciones

                 .OrderByDescending(c => c.Id)
                 .ToListAsync();
                var results = new
                {
                    IsSuccess = true,
                    direcciones
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
