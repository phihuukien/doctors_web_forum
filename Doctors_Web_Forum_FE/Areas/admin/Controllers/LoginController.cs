using Doctors_Web_Forum_FE.BusinessModels;
using Doctors_Web_Forum_FE.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Areas.admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    public class LoginController : Controller
    {
        private readonly DatabaseContext _context;
        public LoginController(DatabaseContext context)
        {
            _context = context;
        }

        // Redirect admin login page
        [Route(""),Route("login")]
        public IActionResult Index()
        {
            return View("Login");
        }

        // login authentication
        [Route("authentication")]
        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            if (null == Email || null == Password)
            {
                TempData["error"] = "Email or Password Empty";
                return Redirect("~/admin/login");
            }

            var md5pass = Utility.MD5Hash(Password);
            var account = _context.Accounts.FirstOrDefault(x => x.Email == Email && x.Password == md5pass);
            if (account != null)
            {
                if ((account.Status == 1 || account.Status == 3)  && account.Role.Equals("ADMIN"))
                {
                    string id = account.AccountId.ToString();
                    var identity = new ClaimsIdentity(new[]
                   {
                      new Claim(ClaimTypes.Email ,account.Email),
                      new Claim(ClaimTypes.NameIdentifier ,account.DisplayName),
                      new Claim(ClaimTypes.Role,account.Role),
                      new Claim("Avatar",account.Avatar),
                      new Claim("AccountId",id)
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    _context.Accounts.Update(account);
                    await _context.SaveChangesAsync();
                    return Redirect("~/admin/home");
                }
                else
                {
                    if (account.Role.Equals("USER"))
                    {
                        TempData["error"] = "Your account is not correct";
                        return Redirect("/admin/login");
                    }
                    TempData["error"] = "Account has not been activated";
                    return Redirect("/admin/login");
                }
            }
            else
            {
                TempData["email"] = Email;
                TempData["password"] = Password;
                TempData["error"] = "Email or Password Incorrect";
                return Redirect("/admin/login");
            }
        }

        // logout
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var id = @User.Claims.Skip(4).FirstOrDefault().Value;
            var accountId = Int32.Parse(id);
            var account = _context.Accounts.FirstOrDefault(x => x.AccountId == accountId);
            account.Status = 1;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/admin/login");

        }
    }
}
