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
    public class ProviderController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ProviderController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task<IActionResult> GetProviders()
        {
           

            try
            {
                List<Provider> providers = await _context.Providers
             .OrderBy(p => p.nombre)
             .ToListAsync();
                var results = new
                {
                    IsSuccess = true,
                    providers
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
        public async Task<IActionResult> PostProvider([FromBody] Provider request)
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
                Provider provider = new Provider
                {
                    nombre = request.nombre,
                    rnc = request.rnc,
                    direccion = request.direccion,
                    telefono = request.telefono,
                    correo = request.correo,
                    web = request.web,
                    tipo = request.tipo,
                    logo = request.logo,

                };


                _context.Providers.Add(provider);

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

        public async Task<IActionResult> EditProvider([FromBody] Provider request)
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
                var itemToEdit = _context.Providers.Single(o => o.Id == request.Id);
                if (itemToEdit != null)
                {

                    itemToEdit.nombre = request.nombre;
                    itemToEdit.rnc = request.rnc;
                    itemToEdit.direccion = request.direccion;
                    itemToEdit.telefono = request.telefono;
                    itemToEdit.correo = request.correo;
                    itemToEdit.web = request.web;
                    itemToEdit.tipo = request.tipo;
                    itemToEdit.logo = request.logo;

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
        [Route("deleteProvider")]
        public async Task<IActionResult> DeleteProvider([FromBody] Provider request)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso para realizar esta operación.!" });
            }

            try
            {
                var itemToDelete = _context.Providers.Single(o => o.Id == request.Id);
                if (itemToDelete != null)
                {
                    _context.Providers.Remove(itemToDelete);
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
