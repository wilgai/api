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
    public class ClientController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        public ClientController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task<IActionResult> GetClients()
        {
            try
            {
                List<Client> clients = await _context.Clients
               .OrderBy(p => p.nombre)
               .ToListAsync();
                var results = new
                {
                    IsSuccess = true,
                    clients
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
        public async Task<IActionResult> PostClient([FromBody] Client request)
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
                Client client = new Client
                {
                    nombre = request.nombre,
                    direccion = request.direccion,
                    identificacion = request.identificacion,
                    telefono = request.telefono,
                    correo = request.correo,
                    sexo = request.sexo,
                    celular = request.celular
                };



                _context.Clients.Add(client);
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

        public async Task<IActionResult> EditClient([FromBody] Client request)
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
                var itemToEdit = _context.Clients.Single(o => o.Id == request.Id);
                if (itemToEdit != null)
                {
                    itemToEdit.nombre = request.nombre;
                    itemToEdit.direccion = request.direccion;
                    itemToEdit.identificacion = request.identificacion;
                    itemToEdit.telefono = request.telefono;
                    itemToEdit.correo = request.correo;
                    itemToEdit.sexo = request.sexo;
                    itemToEdit.celular = request.celular;

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
        [Route("deleteClient")]
        public async Task<IActionResult> DeleteClient([FromBody] Client request)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso para realizar esta operación.!" });
            }

            try
            {
                var itemToDelete = _context.Clients.Single(o => o.Id == request.Id);
                if (itemToDelete != null)
                {
                    _context.Clients.Remove(itemToDelete);
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
