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
    public class ModelController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ModelController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetModels()
        {
           
            try
            {
                var models = (from m in _context.Models
                              select new
                              {
                                  m.Id,
                                  m.nombre,
                                  m.brandId,
                              }).ToList();
                var results = new
                {
                    IsSuccess = true,
                    models
                };
                return Ok(results);
            }
            catch (Exception exception)
            {

                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetModelsById(int id)
        {

            try
            {

                var models = (from m in _context.Models
                                join b in _context.Brands on m.brandId equals b.Id
                                where m.Id == id
                                select new
                                {
                                    m.Id,
                                    m.brandId,
                                    m.nombre,
                                    nombreMarca=b.nombre
                                }).ToList();
                var results = new
                {
                    IsSuccess = true,
                    models
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
        public async Task<IActionResult> PostModel([FromBody] Model request)
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
                Model model = new Model
                {
                    nombre = request.nombre,
                    Brand = await _context.Brands.FindAsync(request.brandId)
                };

                _context.Models.Add(model);

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
                    mes = "Este modelo ya existe";

                }

                return Ok(new Response { IsSuccess = response, Message = mes });


            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return Ok(new Response { IsSuccess = false, Message = "Este modelo ya existe" });
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

        public async Task<IActionResult> EditModel([FromBody] Model request)
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
               
                var itemToEdit = _context.Models.Single(o => o.Id == request.Id);
                if (itemToEdit != null)
                {
                    itemToEdit.nombre = request.nombre;
                    itemToEdit.Brand = await _context.Brands.FindAsync(request.brandId);

                }

                _context.SaveChanges();

                return Ok(new Response { IsSuccess = true, Message = "Se ha guardo los cambios exitosamente." });


            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return Ok(new Response { IsSuccess = false, Message = "Este modelo  ya existe" });
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
        [Route("deleteModel")]
        public async Task<IActionResult> DeleteModel([FromBody] Model request)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no tiene permiso para realizar esta operación.!" });
            }

            try
            {
                

                var itemToDelete = _context.Models.Single(o => o.Id == request.Id);
                if (itemToDelete != null)
                {
                    _context.Models.Remove(itemToDelete);
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
