
using Doctors_Web_Forum_FE.BusinessModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        public HomeController(DatabaseContext context)
        {
            _context = context;
        }
        [Route("")]
        public IActionResult Index()
        {
            var isActive = _context.Accounts.Where(x => x.Status == 3).Count();
            var registed = _context.Accounts.Where(x => x.Status == 1 || x.Status == 3).Count();
            HttpContext.Session.SetString("isAction", isActive.ToString());
            HttpContext.Session.SetString("registed", registed.ToString());
            return View();
        }
    }
}
