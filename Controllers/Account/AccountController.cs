using CrudApplication.Data;
using CrudApplication.Models.Account;
using CrudApplication.Models.ViewModel;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CrudApplication.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext context;
        private readonly IDNTCaptchaValidatorService validatorService;

        public AccountController(ApplicationContext context,IDNTCaptchaValidatorService validatorService)
        {
            this.context = context;
            this.validatorService = validatorService;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        //[ValidateDNTCaptcha(ErrorMessage ="Please enter security code!",CaptchaGeneratorLanguage =Language.English,CaptchaGeneratorDisplayMode =DisplayMode.ShowDigits)]
        public IActionResult Login(LoginSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                {
                    TempData["captchaError"] = "Please enter valid security key";
                    return View(model);
                }
                var data = context.Users.Where(e => e.Username == model.Username).SingleOrDefault();
                if (data != null)
                {
                    bool isValid = (data.Username == model.Username && DecryptPassword(data.Password) == model.Password);
                    if (isValid)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.Username) }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        HttpContext.Session.SetString("Username", model.Username);
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        TempData["errorPassword"] = "Invalid password!";
                        return View(model);
                    }
                }
                else
                {
                    TempData["errorUsername"] = "Username not found!";
                    return View(model);
                    //this is async coojnfdfd
                }
            }
            else
            {
                return View(model);
            }

        }

        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] storePassword = ASCIIEncoding.ASCII.GetBytes(password);
                string encryptedPassword = Convert.ToBase64String(storePassword);
                return encryptedPassword;
            }
        }

        public static string DecryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] encryptedPassword = Convert.FromBase64String(password);
                string decryptedPassword = ASCIIEncoding.ASCII.GetString(encryptedPassword);
                return decryptedPassword;
            }
        }


        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach (var cookies in storedCookies)
            {
                Response.Cookies.Delete(cookies);
            }
            return RedirectToAction("Login", "Account");
        }


        [AcceptVerbs("Post", "Get")]
        public IActionResult UserNameIsExist(string userName)
        {
            var data = context.Users.Where(e => e.Username == userName).SingleOrDefault();
            if (data != null)
            {
                return Json($"Username {userName} already in use!");
            }
            else
            {
                return Json(true);
            }
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(SignUpUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new User()
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = EncryptPassword(model.Password),
                    Mobile = model.Mobile,
                    IsActive = model.IsActive
                };
                context.Users.Add(data);
                context.SaveChanges();
                TempData["successMessage"] = "You are eligible to login, Please fill own credential's then login!";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["errorMessage"] = "Empty form can't be submitted!";
                return View(model);
            }

        }


    }
}
