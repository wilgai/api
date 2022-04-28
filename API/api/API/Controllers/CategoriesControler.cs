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
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public CategoriesController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            
            try
            {
                List<Category> categories = await _context.Categories
               .OrderBy(p => p.nombre)
               .ToListAsync();
                var results = new
                {
                    IsSuccess = true,
                    categories
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

        public async Task<IActionResult> PostCategory([FromBody] Category request)
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
                Category category = new Category
                {
                    nombre = request.nombre,
                };

                _context.Categories.Add(category);


                _context.SaveChanges();


                return Ok(new Response { IsSuccess = true, Message = "Se ha guardo los cambios exitosamente" });


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

        public async Task<IActionResult> EditCategory([FromBody] Category request)
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
                Category category = new Category
                {
                    nombre = request.nombre,
                };

                var itemToEdit = _context.Categories.Single(o => o.Id == request.Id);
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
        [HttpPost]
        [Route("deleteCategory")]
        public async Task<IActionResult> DeleteProvider([FromBody] Category request)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso para realizar esta operación.!" });
            }

            try
            {
                Category category = new Category
                {
                    nombre = request.nombre,
                };

                var itemToDelete = _context.Categories.Single(o => o.Id == request.Id);
                if (itemToDelete != null)
                {
                    _context.Categories.Remove(itemToDelete);
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
