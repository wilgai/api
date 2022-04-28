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
    public class ClientesController : ControllerBase
    {
        private readonly DataContext _context;


        public ClientesController(DataContext context)
        {
            _context = context;
        }
        [HttpPut]
        public async Task<IActionResult> ActualizarCliente(Cliente request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }
            try
            {
                var clienteParaEditar = _context.Clientes.FirstOrDefault(o => o.Id == request.Id);

                if (clienteParaEditar != null)
                {
                    clienteParaEditar.Nombre = request.Nombre;
                    clienteParaEditar.Identificacion = request.Identificacion;
                    clienteParaEditar.Telefono = request.Telefono;
                    clienteParaEditar.Celular = request.Celular;
                    clienteParaEditar.Sexo = request.Sexo;
                    clienteParaEditar.Imagen = request.Imagen;
                    clienteParaEditar.Correo = request.Correo;
                }
                else
                {
                    return Ok(new Response { IsSuccess = false, Message = "Resgistro no existe" });
                }

                //Actualizar Direcciones
                foreach (Direccion item in request.Direcciones.ToList())
                {
                    if(item.Id == 0 )
                    {
                        clienteParaEditar.Direcciones.Add(new Direccion
                        {
                            LineaDeDireccion1 = item.LineaDeDireccion1,
                            LineaDeDireccion2 = item.LineaDeDireccion2,
                            Pais = item.Pais,
                            Provicia = item.Provicia,
                            Municipio = item.Municipio,
                            CodigoPostal = item.CodigoPostal,
                        });

                    }
                    else if (item.Id > 0)
                    {
                        var direccionParaEditar = _context.Direcciones.Single(o => o.Id == item.Id);
                        if (direccionParaEditar != null)
                        {
                            direccionParaEditar.LineaDeDireccion1 = item.LineaDeDireccion1;
                            direccionParaEditar.LineaDeDireccion2 = item.LineaDeDireccion2;
                            direccionParaEditar.Pais = item.Pais;
                            direccionParaEditar.Provicia = item.Provicia;
                            direccionParaEditar.Municipio = item.Municipio;
                            direccionParaEditar.CodigoPostal = item.CodigoPostal;
                        }
                    }

                }

                foreach (ConocimientoDedesarrollo item in request.Conocimientos.ToList())
                {   if(item.Id == 0)
                    {
                        clienteParaEditar.Conocimientos.Add(new ConocimientoDedesarrollo
                        {
                            Conocimiento = item.Conocimiento,
                        });
                    }
                    if (item.Id > 0)
                    {
                        var conocimientoParaEditar = _context.Conocimientos.Single(o => o.Id == item.Id);
                        if (conocimientoParaEditar != null)
                        {
                            if (conocimientoParaEditar != null)
                            {
                                conocimientoParaEditar.Conocimiento = item.Conocimiento;
                            }
                        }
                    }
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
        [Route("eliminarCliente")]
        public async Task<IActionResult> EliminarCliente(Cliente request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }

            try
            {
                var clienteParaEliminar = _context.Clientes.FirstOrDefault(o => o.Id == request.Id);
                if (clienteParaEliminar != null)
                {


                    clienteParaEliminar.Estado = "INACTIVO";
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
                    mes = "El cliente ha sido cancelado.";
                }
                else
                {
                    response = false;
                    mes = "Hubo un problema al eliminar el cliente.";

                }

                return Ok(new Response { IsSuccess = response, Message = mes });
            }
            catch (Exception exception)
            {
                return Ok(new Response { IsSuccess = false, Message = exception.InnerException.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> GuardarCliente([FromBody] Cliente request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }
            try
            {
                Cliente cliente = new Cliente
                {

                    Nombre = request.Nombre,
                    Identificacion = request.Identificacion,
                    Correo = request.Correo,
                    Celular = request.Celular,
                    Telefono = request.Telefono,
                    Sexo = request.Sexo,
                    Imagen = request.Imagen,
                    Estado = "ACTIVO",
                    Direcciones = new List<Direccion>(),
                    Conocimientos = new List<ConocimientoDedesarrollo>()

                };
                _context.Clientes.Add(cliente);
                //Insertando Direcciones
                foreach (Direccion item in request.Direcciones)
                {
                    cliente.Direcciones.Add(new Direccion
                    {
                        LineaDeDireccion1 = item.LineaDeDireccion1,
                        LineaDeDireccion2 = item.LineaDeDireccion2,
                        Pais = item.Pais,
                        Provicia = item.Provicia,
                        Municipio = item.Municipio,
                        CodigoPostal = item.CodigoPostal,
                    });

                }
                _context.Clientes.Add(cliente);
                foreach (ConocimientoDedesarrollo item in request.Conocimientos)
                {
                    cliente.Conocimientos.Add(new ConocimientoDedesarrollo
                    {
                        Conocimiento = item.Conocimiento,
                    });

                }
                //Insertando Conocimientos

                _context.Clientes.Add(cliente);

                bool response = false;
                string mes = "";
                var created = await _context.SaveChangesAsync();
                if (created > 0)
                {
                    response = true;
                    mes = "Se ha creado el cliente exitosamente";
                }
                else
                {
                    response = false;
                    mes = "Hubo un problema al guardar el cliente";

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
        public async Task<IActionResult> ObtenerClientes()
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea." });
            }
            try
            {
                List<Cliente> clientes = await _context.Clientes
                  
                 .OrderByDescending(c => c.Id)
                 .ToListAsync();
                var results = new
                {
                    IsSuccess = true,
                    clientes
                };
                return Ok(results);
            }
            catch (Exception exception)
            {

                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerClientePorID(string id)
        {
            try
            {
                var clientes = (from c in _context.Clientes.AsNoTracking()
                                where c.Id.ToString() == id
                                select new
                                {
                                    c.Id,
                                    c.Nombre,
                                    c.Identificacion,
                                    c.Imagen,
                                    c.Sexo,
                                    c.Telefono,
                                    c.Celular,
                                    c.Correo
                                }).ToList();

                var direcciones = (from d in _context.Direcciones.AsNoTracking()
                                   where d.Cliente.Id.ToString() == id
                                   select new
                                   {
                                       d.Id,
                                       d.LineaDeDireccion1,
                                       d.LineaDeDireccion2,
                                       d.Pais,
                                       d.Provicia,
                                       d.Municipio,
                                       d.CodigoPostal
                                   }).ToList();
                var conocimientos = (from c in _context.Conocimientos.AsNoTracking()
                                     where c.Cliente.Id.ToString() == id
                                     select new
                                     {
                                         c.Id,
                                         c.Conocimiento,
                                     }).ToList();
                var results = new
                {
                    IsSuccess = true,
                    clientes,
                    direcciones,
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
