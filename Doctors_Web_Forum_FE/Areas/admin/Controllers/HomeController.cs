using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Areas.admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    public class HomeController : Controller
    {

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
