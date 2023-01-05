using Doctors_Web_Forum_FE.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using MailKit.Security;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctors_Web_Forum_FE.BusinessModels;
using System.Text;
using System.Security.Cryptography;

using Doctors_Web_Forum_FE.Services;
using MailKit;
using Doctors_Web_Forum_FE.Util;

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
        // login
        [Route("")]
        public IActionResult Login()
        {
            return View("Login");
        }

        //Register and Send Email
        [HttpPost]
        [Route("postRegister")]
        public async Task<IActionResult> PostRegister(string check_password, [Bind("DisplayName,Email,Password")] Account account)
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
