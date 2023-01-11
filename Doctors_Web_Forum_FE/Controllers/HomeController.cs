
using Doctors_Web_Forum_FE.BusinessModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Doctors_Web_Forum_FE.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        public HomeController(DatabaseContext context)
        {
            _context = context;
        }
        // get new question and recent Question 
        [Route("")]
        public IActionResult Index(int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pageSize = 5;
            var isActive = _context.Accounts.Where(x => x.Status == 3).Count();
            var registed = _context.Accounts.Where(x => x.Status == 1 || x.Status == 3).Count();
            HttpContext.Session.SetString("isAction", isActive.ToString());
            HttpContext.Session.SetString("registed", registed.ToString());
            var questions = _context.Questions.Include(T => T.Topic).Include(A => A.Account).Where(x=>x.CreateDate.Day == DateTime.Now.Day && x.Status == true).OrderByDescending(Q => Q.CreateDate).ToPagedList(page, pageSize); ;
            ViewBag.recentQuestion = questions.Count();
            if (questions.Count() == 0 )
            {
                questions = _context.Questions.Include(T => T.Topic).Include(A => A.Account).Where(x =>  x.Status == true).OrderByDescending(Q => Q.CreateDate).Take(20).ToPagedList(page, pageSize);
            }
            return View(questions);
        }
    }
}
