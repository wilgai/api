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
    public class ConfigurationsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ConfigurationsController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetConfiguration()
        {
           

            try
            {
                List<Configuration> configuration = await _context.Configurations
             .ToListAsync();
                var results = new
                {
                    IsSuccess = true,
                    configuration
                };
                return Ok(results);
            }
            catch (Exception exception)
            {

                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }

        }
        [HttpPost]
        public async Task<IActionResult> PostConfiguration([FromBody] Configuration request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Informacion errónea." });
            }
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso." });
            }
            try
            {
                Configuration configuration = new Configuration
                {
                    empresa = request.empresa,
                    direccion = request.direccion,
                    telefono = request.telefono,
                    correo = request.correo,
                    celular = request.celular,
                    web = request.web,
                    whatsap = request.whatsap,
                    facebook = request.facebook,
                    instagram = request.instagram,
                    tiktok = request.tiktok,
                    logo = request.logo,
                    notaFactura = request.notaFactura,
                    notaOrdenDeServicio = request.notaOrdenDeServicio,
                    notaServicio = request.notaServicio,
                    instrucionOdenDeServicio = request.instrucionOdenDeServicio,
                    moneda = request.moneda,
                    notaCotizacionFactura = request.notaCotizacionFactura,
                    notaCotizacionReparacion = request.notaCotizacionReparacion,
                };
                _context.Configurations.Add(configuration);
                _context.SaveChanges();

                return Ok(new Response { IsSuccess = true, Message = "Se ha guardo los cambios exitosamente." });

            }
            catch(Exception exception)
            {
                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }

          
        }

        [HttpPut]
        public async Task<IActionResult> EditConfiguration([FromBody] Configuration request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Informacion errónea." });
            }
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso." });
            }
            try
            {
                var itemToEdit = _context.Configurations.Single(o => o.Id == request.Id);
                if (itemToEdit != null)
                {
                    itemToEdit.empresa = request.empresa;
                    itemToEdit.direccion = request.direccion;
                    itemToEdit.telefono = request.telefono;
                    itemToEdit.correo = request.correo;
                    itemToEdit.celular = request.celular;
                    itemToEdit.web = request.web;
                    itemToEdit.whatsap = request.whatsap;
                    itemToEdit.facebook = request.facebook;
                    itemToEdit.instagram = request.instagram;
                    itemToEdit.tiktok = request.tiktok;
                    itemToEdit.logo = request.logo;
                    itemToEdit.notaFactura = request.notaFactura;
                    itemToEdit.notaOrdenDeServicio = request.notaOrdenDeServicio;
                    itemToEdit.notaServicio = request.notaServicio;
                    itemToEdit.instrucionOdenDeServicio = request.instrucionOdenDeServicio;
                    itemToEdit.moneda = request.moneda;
                    itemToEdit.notaCotizacionFactura = request.notaCotizacionFactura;
                    itemToEdit.notaCotizacionReparacion = request.notaCotizacionReparacion;
                }
                _context.SaveChanges();

                return Ok(new Response { IsSuccess = true, Message = "Se ha guardo los cambios exitosamente." });

            }
            catch (Exception exception)
            {
                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }


        }

    }
}
