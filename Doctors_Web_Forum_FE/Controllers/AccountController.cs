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

namespace Doctors_Web_Forum_FE.Controllers
{

    [Route("account")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Route("profile")]
        public IActionResult Profile()
        {
            return View("Profile");
        }

    }
}