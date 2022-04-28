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
    public class BrandController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public BrandController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task<IActionResult> GetBrands()
        {
            
            try
            {
                List<Brand> brands = await _context.Brands
               .OrderBy(p => p.nombre)
               .ToListAsync();
                var results = new
                {
                    IsSuccess = true,
                    brands
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

        public async Task<IActionResult> PostBrand([FromBody] Brand request)
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
                Brand brand = new Brand
                {
                    nombre = request.nombre,
                };

                _context.Brands.Add(brand);

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
                    mes = "Este nombre de categoria ya existe";

                }

                return Ok(new Response { IsSuccess = response, Message = mes });


            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return Ok(new Response { IsSuccess = false, Message = "Este nombre de categoria ya existe" });
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

        public async Task<IActionResult> EditBrand([FromBody] Brand request)
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
                Brand brand = new Brand
                {
                    nombre = request.nombre,
                };

                var itemToEdit = _context.Brands.Single(o => o.Id == request.Id);
                if (itemToEdit != null)
                {
                    itemToEdit.nombre = request.nombre;

                }
               
                _context.SaveChanges();
                
                return Ok(new Response { IsSuccess = true, Message = "Se ha guardo los cambios exitosamente" });


            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return Ok(new Response { IsSuccess = false, Message = "Esta marca nombre  ya existe" });
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
        [Route("deleteBrand")]
        public async Task<IActionResult> DeleteProvider([FromBody] Brand request)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso para realizar esta operación.!" });
            }

            

            try
            {
                Brand brand = new Brand
                {
                    nombre = request.nombre,
                };

                var itemToDelete = _context.Brands.Single(o => o.Id == request.Id);
                if (itemToDelete != null)
                {
                    _context.Brands.Remove(itemToDelete);
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
