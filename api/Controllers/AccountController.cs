using api.Data;
using api.Entities;
using api.Helper;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        
        private readonly DataContext _context;
        private readonly IMailHelper _mailHelper;
        
        public AccountController(DataContext context, IUserHelper userHelper,  IMailHelper mailHelper)
        {

            _context = context;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
           
        }


        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty,result.ToString());
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RecoverPasswordMVC()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPasswordMVC(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                string link = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);
                _mailHelper.SendMail(model.Email, "Restablecer contraseña", $"<h1>Restablecer contraseña</h1>" +
                    $"Sigue este este enlace para restablecer la contraseña:</br></br>" +
                    $"<a href = \"{link}\">Restablecer consenesa</a>");
                ViewBag.Message = "Las instrucciones para cambiar la contraseña ha sido enviado a su correo.";
                return View();

            }

            return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user != null)
            {
                IdentityResult result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Su contraseña ha sido restablecido, ya puede logearse.";
                    return View();
                }

                ViewBag.Message = "Hubo un error al cambiar la contraseña.";
                return View(model);
            }

            ViewBag.Message = "User not found.";
            return View(model);
        }




    }

}
