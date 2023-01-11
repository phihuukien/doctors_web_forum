using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Doctors_Web_Forum_FE.BusinessModels;
using Doctors_Web_Forum_FE.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using X.PagedList;
using Doctors_Web_Forum_FE.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Doctors_Web_Forum_FE.Util;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Doctors_Web_Forum_FE.Controllers
{
    [Route("users")]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _context;
        public AccountController(DatabaseContext context)
        {
            _context = context;
        }
        // get all user and search
        [Route("")]
        public IActionResult Index(string userName, int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pageSize = 5;
            var account = _context.Accounts.Where(x => x.Status == 1 || x.Status == 3).AsQueryable();
            if (!string.IsNullOrEmpty(userName)) {
                account = account.Where(x => x.DisplayName.ToLower().Contains(userName.ToLower()));
                ViewBag.doctorName = userName;
            }
            var accounts = account.ToPagedList(page, pageSize) ;
            return View(accounts);
            
        }
        [Route("profile/{id}")]
        public IActionResult Profile(int id)
        {
            ViewBag.totalQuestions = _context.Questions.Where(x => x.AccountId == id).Count();
            ViewBag.totalAnswers = _context.Comments.Where(x => x.AccountId == id).Count();
            var account = _context.Accounts.Find(id);
            return View(account);
        }
        [Route("edit-profile/{id}")]
        public IActionResult EditProfile(string id)
        {
           
            var account = _context.Accounts.Find(Int32.Parse(id));
            return View(account);
        }
        [Route("edit-profile")]
        [HttpPost]
        public IActionResult EditProfile(string pass, string old_img, Account account, IFormFile Avatar)
        {
            if (ModelState.IsValid)
            {
                string urlImg = "";
                if (Avatar != null && Avatar.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets/img/" + Avatar.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        Avatar.CopyToAsync(stream);
                    }
                    urlImg = "/assets/img/" + Avatar.FileName;
                    account.Avatar = urlImg;
                }
                else
                {
                    account.Avatar = old_img;
                }
                account.Password = pass;
                account.Status = 3;
                account.Role = "USER";
                account.UpdateDate = DateTime.Now;
                _context.Accounts.Update(account);
                _context.SaveChanges();
                return Redirect("profile/" + account.AccountId);
            }
            return View("EditProfile", account);
        }

        [Route("edit-password")]
        [HttpPost]
        public IActionResult EditPassword(string Password, string old_Password, string Check_Password)
        {
            var accountId = @User.Claims.Skip(4).FirstOrDefault().Value;
            var account = _context.Accounts.Find(Int32.Parse(accountId));
            if (old_Password == null)
            {
                TempData["error1"] = "Password Can Not Be Blank";
                return View("EditProfile", account);
            }
            var md5pass = Utility.MD5Hash(old_Password);
            if (md5pass != account.Password)
            {
                TempData["error1"] = "Incorrect Password";
                return View("EditProfile", account);
            }

            if (Password == null)
            {
                TempData["error2"] = "Password Can Not Be Blank";
                return View("EditProfile", account);
            }

            string pattern = @"^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9]))(?=.*[#$^+=!*()@%&]).{8,}$";
            Regex rg = new Regex(pattern);
            if (rg.IsMatch(Password) != true)
            {
                TempData["error2"] = "Wrong Password Format";
                return View("EditProfile", account);
            }
            var pass = Utility.MD5Hash(Password);

            if (Password != Check_Password)
            {
                TempData["error3"] = "Password Does Not Match";
                return View("EditProfile", account);

            }
            account.Password = pass;
            account.UpdateDate = DateTime.Now;
            _context.Update(account);
            _context.SaveChanges();
            return Redirect("profile/" + account.AccountId);
        }
    }
}