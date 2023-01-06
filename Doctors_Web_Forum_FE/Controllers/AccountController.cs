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

namespace Doctors_Web_Forum_FE.Controllers
{
    [Route("doctor")]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _context;
        public AccountController(DatabaseContext context)
        {
            _context = context;
        }
        [Route("")]
        public IActionResult Index()
        {
            var account = _context.Accounts.Where(x => x.Status == 1 || x.Status == 3).ToList();
            return View(account);
        }
        [Route("profile")]
        public IActionResult Profile()
        {
            return View("Profile");
        }
    }
}