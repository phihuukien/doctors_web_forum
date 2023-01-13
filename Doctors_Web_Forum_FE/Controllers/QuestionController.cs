using Doctors_Web_Forum_FE.BusinessModels;
using Doctors_Web_Forum_FE.Models;
using Doctors_Web_Forum_FE.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Doctors_Web_Forum_FE.Controllers
{
    [Route("question")]
    public class QuestionController : Controller
    {
        private readonly DatabaseContext _context;
        public QuestionController(DatabaseContext context)
        {
            _context = context;
        }

        // get all questions
        [Route("")]
        public IActionResult Question(string title, int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pageSize = 5;

            var questions = _context.Questions.Include(T => T.Topic).Include(A => A.Account).Where(x=>x.Status == true).OrderByDescending(Q => Q.CreateDate).ToPagedList(page, pageSize);
            if (!string.IsNullOrEmpty(title))
            {
                 questions = _context.Questions.Include(T => T.Topic).Include(A => A.Account).Where(x=>x.Title.Contains(title) && x.Status == true).OrderByDescending(Q => Q.CreateDate).ToPagedList(page, pageSize);
                ViewBag.titleSearch = title;
                ViewBag.numberOfQuestionSearch = questions.Count();
            }
            ViewBag.numberOfQuestion = _context.Questions.Count();
            return View(questions);
        }

        // question detail 
        [Route("{id}")]
        public IActionResult QuestionDetail(int id)
        {
           var question  = _context.Questions.Include(T => T.Topic).Include(A => A.Account).Where(Q => Q.QuestionId == id).FirstOrDefault();
            ViewBag.selecTime = Utility.SelecTime(question.CreateDate);
            ViewData["Question"] = question;
            ViewData["numberOfAnswers"] = _context.Comments.Where(c => c.QuestionId == id).Count();
            var comments = _context.Comments.Where(c => c.QuestionId == id || c.Reply == c.CommentId).Include(A => A.Account).ToList();
            return View(comments);
        }

        // get all topic to form add new question 
        [Route("get-topic")]
        public IActionResult GetTopics()
        {
            var topics = _context.Topics.Where(x=>x.Status == true).AsEnumerable();
            return new JsonResult(topics);
        }

        // insert new question 
        [Authorize(Roles = "USER")]
        [Route("insert")]
        [HttpPost]
        public async Task<IActionResult> Insert([FromForm] Question question )
        {
            string accountId = @User.Claims.Skip(4).FirstOrDefault().Value;
            question.CreateDate = DateTime.Now;
            question.UpdateDate = DateTime.Now;
            question.Status = true;
            question.AccountId = Int32.Parse(accountId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var questionExits = _context.Questions.Where(x => x.Title == question.Title).FirstOrDefault();
            if (questionExits != null)
            {
                return BadRequest(new { message = "Question Duplicate" });
            }
             _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return Ok(new { message="Success Insert", id= question.QuestionId });
        }

        // insert comment
        [Authorize(Roles = "USER")]
        [Route("post-Comment")]
        public async Task<IActionResult>  PostComment([FromForm] Comment comment)
        {
            // update totalQuestion in question table
            var question = _context.Questions.Where(Q => Q.QuestionId == comment.QuestionId).FirstOrDefault();
            int totalQuestions = question.TotalQuestion;
            question.TotalQuestion = totalQuestions + 1;
            _context.Questions.Update(question);
            // insert answer
            string accountId = @User.Claims.Skip(4).FirstOrDefault().Value;
            comment.AccountId = Int32.Parse(accountId);
            comment.CreateDate = DateTime.Now;
            comment.UpdateDate = DateTime.Now;
            comment.Status = true;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Success Comment" });
        }
    }
}
