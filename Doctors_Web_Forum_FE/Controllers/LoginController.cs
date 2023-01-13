using Doctors_Web_Forum_FE.BusinessModels;
using Doctors_Web_Forum_FE.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Doctors_Web_Forum_FE.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Doctors_Web_Forum_FE.Services;
using Microsoft.AspNetCore.Http;

namespace Doctors_Web_Forum_FE.Controllers
{

    [Route("login")]
    public class LoginController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IEmailService mailService;
        public LoginController(DatabaseContext context, IEmailService mailService)
        {
            _context = context;
            this.mailService = mailService;
        }

        //Redirect login page
        [Route("", Name = "LogIn")]
        public IActionResult Login(string urlRedirect)
        {
            ViewBag.urlRedirect = urlRedirect;
            return View("Login");
        }

        // login authentication
        [Route("authentication")]
        [HttpPost]
        public async Task<IActionResult> LoginAuthen(string Email, string Password, string UrlRedirect)
        {
            // check null
            if (null == Email || null == Password)
            {
                TempData["error"] = "UserName or Password Empty";
                return Redirect("~/login");
            }
            var md5pass = Utility.MD5Hash(Password);
            // check account exist in database
            var account = _context.Accounts.FirstOrDefault(x => x.Email == Email && x.Password == md5pass);
            if (account != null)
            {
                // check account status (1<=>activated , 2 <=> not activated , 3 <=> activated and is active , 4 <=> lock) 
                if ((account.Status == 1 || account.Status == 3) && account.Role.Equals("USER"))
                {
                    // add imformation to cookie
                    string id = account.AccountId.ToString();
                    var identity = new ClaimsIdentity(new[]  {
                      new Claim(ClaimTypes.Email ,account.Email),
                      new Claim(ClaimTypes.NameIdentifier ,account.DisplayName),
                      new Claim(ClaimTypes.Role,account.Role),
                      new Claim("Avatar",account.Avatar),
                      new Claim("AccountId",id)
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    UrlRedirect = string.IsNullOrEmpty(UrlRedirect) ? "/" : UrlRedirect;
                    account.Status = 3;
                    HttpContext.Session.SetString("img", account.Avatar);
                    _context.Accounts.Update(account);
                    await _context.SaveChangesAsync();
                    return Redirect("~" + UrlRedirect);
                }
                else
                {
                    if (account.Role.Equals("ADMIN"))
                    {
                        TempData["error"] = "Email or Password Incorrect";
                        return Redirect("~/login");
                    }
                    if (account.Status ==4 )
                    {
                        TempData["error"] = "Account has not been lock";
                        return Redirect("~/login");
                    }
                    TempData["error"] = "Account has not been activated";
                    return Redirect("~/login");
                }
            }
            else
            {
                TempData["email"] = Email;
                TempData["password"] = Password;
                TempData["error"] = "Email or Password Incorrect";
                return Redirect("~/login");
            }

        }
      
        //Register and Send Email
        [HttpPost]
        [Route("postRegister")]
        public async Task<IActionResult> PostRegister(string check_password, Account account)
        {
            ViewBag.panelActive = "right-panel-active";
            //check validate entity framework model
            if (ModelState.IsValid)
            {
                var acc = _context.Accounts.Where(a => a.Email.Contains(account.Email)).FirstOrDefault();
                // check unique email
                if (acc == null)
                {
                    //check confirm password
                    if (check_password == account.Password)
                    {
                        DateTime localDate = DateTime.Now;
                        account.CreateDate = localDate;
                        account.UpdateDate = localDate;
                        account.Avatar = "/assets/img/160x160/img1.jpg";
                        account.Role = "USER";
                        account.Status = 2;
                        account.Token = AccountServics.RederToken();
                        var md5pass = Utility.MD5Hash(account.Password);
                        account.Password = md5pass;
                        _context.Add(account);
                        await _context.SaveChangesAsync();
                        await mailService.SendEmailAsync(account.Email, account.Token);
                        TempData["done"] = "Sign Up Success,Please Check Your Email";
                        return RedirectToAction(nameof(Login));
                    }
                    else
                    {
                        ViewBag.checkpassword = "password does not match";
                        return View("Login", account);
                    }
                }
                else
                {
                    ViewBag.checkemail = "The Email was registered";
                    return View("Login", account);
                }

            }
            return View("Login", account);
        }
        // logout
        [Route("logout", Name = "LogOut")]
        public async Task<IActionResult> Logout()
        {
            // update status
            var id = @User.Claims.Skip(4).FirstOrDefault().Value;
            var accountId = Int32.Parse(id);
            var account = _context.Accounts.FirstOrDefault(x => x.AccountId == accountId);
            account.Status = 1;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            // delete cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");

        }

        //Account activation via email
        [Route("active-accounts/{email}/{token}")]
        public async Task<IActionResult> ActiveAccount(string email, string token)
        {
            var account = _context.Accounts.Where(a => a.Email == email).FirstOrDefault();
            if (token == account.Token)
            {
                account.Token = null;
                account.Status = 1;
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
        }
    }
}
