using api.Data;
using api.Entities;
using api.Helper;
using api.Models;
using api.Request;
using api.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace api.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;
        private readonly DataContext _context;
        private JwtSecurityToken token;

        public AccountController(
            IUserHelper userHelper,
            IConfiguration configuration,
            IMailHelper mailHelper,
            DataContext context)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _context = context;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
          
            try
            {
                List<User> users = await _context.Users
             .OrderBy(p => p.nombre)
             .ToListAsync();
                var results = new
                {
                    IsSuccess = true,
                    users
                };
                return Ok(results);
            }
            catch (Exception exception)
            {

                return Ok(new Response { IsSuccess = false, Message = exception.Message });
            }



        }


        [HttpPost]
        [Route("CreateToken")]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Username);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.ValidatePasswordAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        Claim[] claims = new[]
                        {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        token = new JwtSecurityToken(
                           _configuration["Tokens:Issuer"],
                           _configuration["Tokens:Audience"],
                           claims,
                           expires: DateTime.UtcNow.AddDays(1),
                           signingCredentials: credentials);
                        var results = new
                        {
                            IsSuccess = true,
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            user

                        };

                        return Created(string.Empty, results);
                    }
                    else
                    {
                        return Ok(new Response { IsSuccess = false, Message ="Usuario o contraseña incorrecto" });
                    }
                }
            }

            return Ok(new Response { IsSuccess = false, Message = "Usuario o contraseña incorrecto" });
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetUserByUserName")]
        public async Task<IActionResult> GetUserByUserName([FromBody] UserNameRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User user = await _userHelper.GetUserAsync(request.userName);
            if (user == null)
            {
                return NotFound("Error001");
            }

            return Ok(user);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User user = await _userHelper.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> PostUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea!" });
            }

            User user = await _userHelper.GetUserByEmailAsync(request.email);
            if (user != null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Este correo ya existe!" });
            }
            User us = await _userHelper.GetUserAsync(request.userName);
            if (us != null)
            {

                return Ok(new Response { IsSuccess = false, Message = "Este nombre de usuario no es válido!" });
            }

            //TODO: Translate ErrorXXX literals




            user = new User
            {
                direccion = request.direccion,
                identificacion = request.identificacion,
                Email = request.email,
                nombre = request.nombre,
                apellidos = request.apellidos,
                PhoneNumber = request.phoneNumber,
                UserName = request.userName,
                foto = request.foto,
                tipo_usuario = request.tipo_usuario,
                estado = request.estado,
                sexo = request.sexo,
                PasswordHash = request.passwordHash
            };


            IdentityResult result = await _userHelper.AddUserAsync(user, request.passwordHash);
            if (result != IdentityResult.Success)
            {
                return Ok(new Response { IsSuccess = false, Message = result.Errors.FirstOrDefault().Description });
            }

            User userNew = await _userHelper.GetUserByEmailAsync(request.email);
            await _userHelper.AddUserToRoleAsync(userNew, user.tipo_usuario.ToString());

            string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            string tokenLink = Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            _mailHelper.SendMail(request.email, "Email Confirmation", $"<h1>Email Confirmation</h1>" +
                $"To confirm your email please click on the link<p><a href = \"{tokenLink}\">Confirm Email</a></p>");

            return Ok(new Response { IsSuccess = true, Message= "Se ha registrado el usuario con exito! Verifique su correo correo para confirmar la cuenta." });
        }

        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea!" });
            }

            User user = await _userHelper.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no existe." });
            }

            string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);
            _mailHelper.SendMail(request.Email, "Recuperar Contraseña", $"<h1>Recuperar Contraseña</h1>" +
                $"Sigue este enlace para restablecer la contraseña:<p>" +
                $"<a href = \"{link}\">Cambiar contraseña</a></p>");

            return Ok(new Response { IsSuccess = true, Message = "Las instrucciones para recuperar la contraseña ha sido enviado a su correo." });
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [Route("PutUser")]
        public async Task<IActionResult> PutUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea!" });
            }
            User user =  _context.Users.Single(o => o.Id == request.id);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no existe." });
            }
            user.direccion = request.direccion;
            user.identificacion = request.identificacion;
            user.Email = request.email;
            user.nombre = request.nombre;
            user.apellidos = request.apellidos;
            user.PhoneNumber = request.phoneNumber;
            user.UserName = request.userName;
            user.foto = request.foto;
            user.estado = request.estado;
            user.sexo = request.sexo;

            IdentityResult respose = await _userHelper.UpdateUserAsync(user);
            if (!respose.Succeeded)
            {
                return Ok(new Response { IsSuccess = false, Message= respose.Errors.FirstOrDefault().Description});
            }
            else
            {
                return Ok(new Response { IsSuccess = true, Message = "Se ha actualizado el usuario con exito." });
            }

           
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [Route("CancelUser")]
        public async Task<IActionResult> CancelUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new Response { IsSuccess = false, Message = "Información erronea!" });
            }
            User user = _context.Users.Single(o => o.Id == request.id);
            if (user == null)
            {
                return Ok(new Response { IsSuccess = false, Message = "Usuario no existe." });
            }
            
            user.estado = false;
            

            IdentityResult respose = await _userHelper.UpdateUserAsync(user);
            if (!respose.Succeeded)
            {
                return Ok(new Response { IsSuccess = false, Message = respose.Errors.FirstOrDefault().Description });
            }
            else
            {
                return Ok(new Response { IsSuccess = true, Message = "Se ha cancelado el usuario con exito." });
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            IdentityResult result = await _userHelper.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {

                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Error005"
                });
            }

            return Ok(new Response { IsSuccess = true });
        }
    }

}
