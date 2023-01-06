using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Areas.admin.Controllers
{
    [Authorize]
    [Area("admin")]
    [Route("admin/account")]
    public class AccountController : Controller
    {
        [Route("")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()

        {
            return View("Account");
        }
    }
}
