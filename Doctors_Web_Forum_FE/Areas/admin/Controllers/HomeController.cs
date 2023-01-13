using Doctors_Web_Forum_FE.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Doctors_Web_Forum_FE.Areas.admin.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Area("admin")]
    [Route("admin/home")]
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        // system statistics
        [Route("")]
        [Authorize]
        public IActionResult Index()
        {
            ViewData["questionsTotal"] =_context.Questions.Count();
            ViewData["answersTotal"] = _context.Comments.Count();
            ViewData["UserTotal"] = _context.Accounts.Where(x=>x.Status == 1 || x.Status== 3).Count();
            ViewData["UserTotalLoked"] =  _context.Accounts.Where(x => x.Status == 4).Count();
            ViewData["UserNotActive"] = _context.Accounts.Where(x => x.Status == 2).Count();
            ViewData["questionsToday"] = _context.Questions.Where(x => x.CreateDate.Day == DateTime.Now.Day).Count();
            return View();
        }
    }
}
