using Doctors_Web_Forum_FE.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Doctors_Web_Forum_FE.Areas.admin.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Area("admin")]
    [Route("admin/questions")]
    public class QuestionController : Controller
    {

        private readonly DatabaseContext _context;
        public QuestionController(DatabaseContext context)
        {
            _context = context;
        }

        // list question
        [Route("")]
        public IActionResult Index(string title,int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pageSize = 5;

            var questions = _context.Questions.Include(T => T.Topic).Include(A => A.Account).OrderByDescending(Q => Q.CreateDate).ToPagedList(page, pageSize);
            if (!string.IsNullOrEmpty(title))
            {
                questions = _context.Questions.Include(T => T.Topic).Include(A => A.Account).Where(x => x.Title.Contains(title)).OrderByDescending(Q => Q.CreateDate).ToPagedList(page, pageSize);
                ViewBag.titleSearch = title;
            }
            return View(questions);
        }

        // lock question
        [Route("lock/{id}")]
        [Authorize]
        public async Task<IActionResult> QuestionLock(int id)
        {
            var question = _context.Questions.Where(x => x.QuestionId == id).FirstOrDefault();
            question.Status = false;
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // receive question
        [Route("pass/{id}")]
        [Authorize]
        public async Task<IActionResult> AccountUnLock(int id)
        {
            var question = _context.Questions.Where(x => x.QuestionId == id).FirstOrDefault();
            question.Status = true;
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
