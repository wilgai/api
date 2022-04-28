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
    public class ConocimientoController : Controller
    {
        private readonly DataContext _context;

        public ConocimientoController(DataContext context)
        {
            _context = context;
        }
        [HttpPut]
        public async Task<IActionResult> ActualizarDireccion(ConocimientoDedesarrollo request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }
            try
            {
                var conocimientoParaEditar = _context.Conocimientos.FirstOrDefault(o => o.Id == request.Id);
               
                if (conocimientoParaEditar != null)
                {
                    conocimientoParaEditar.Conocimiento = request.Conocimiento;
                   
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
        [Route("eliminarConocimiento")]
        public async Task<IActionResult> EliminarCliente(ConocimientoDedesarrollo request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }

            try
            {
                var conocimientoParaEliminar = _context.Conocimientos.FirstOrDefault(o => o.Id == request.Id);
                if (conocimientoParaEliminar != null)
                {

                    _context.Conocimientos.Remove(conocimientoParaEliminar);
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
                    mes = "el registro ha sido eliminado.";
                }
                else
                {
                    response = false;
                    mes = "Hubo un problema al eliminar registro.";

                }

                return Ok(new Response { IsSuccess = response, Message = mes });
            }
            catch (Exception exception)
            {
                return Ok(new Response { IsSuccess = false, Message = exception.InnerException.Message });
            }

        }
        [HttpPost]
        public async Task<IActionResult> GuardarConocimiento([FromBody] ConocimientoDedesarrollo request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }
            try
            {

                ConocimientoDedesarrollo conocimiento = new ConocimientoDedesarrollo
                {
                    Cliente = await _context.Clientes.FindAsync(request.codigoCliente),
                    Conocimiento = request.Conocimiento,
                };
                _context.Conocimientos.Add(conocimiento);
                bool response = false;
                string mes = "";
                var created = await _context.SaveChangesAsync();
                if (created > 0)
                {
                    response = true;
                    mes = "Se ha creado el registro exitosamente";
                }
                else
                {
                    response = false;
                    mes = "Hubo un problema al guardar registro";

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
        public async Task<IActionResult> ObtenerConocimientos()
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }
            try
            {
                List<ConocimientoDedesarrollo> conocimientos = await _context.Conocimientos

                 .OrderByDescending(c => c.Id)
                 .ToListAsync();
                var results = new
                {
                    IsSuccess = true,
                    conocimientos
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
