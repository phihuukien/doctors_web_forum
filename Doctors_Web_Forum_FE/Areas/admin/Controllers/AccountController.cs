using Doctors_Web_Forum_FE.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Doctors_Web_Forum_FE.Areas.admin.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Area("admin")]
    [Route("admin/accounts")]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _context;
        public AccountController(DatabaseContext context)
        {
            _context = context;
        }
        // view list users(account)
        [Route("")]
        [Authorize]
        public IActionResult Account(string email, int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pageSize = 5;

            var accounts = _context.Accounts.Where(x => x.Role == "USER").AsQueryable();
            if (!string.IsNullOrEmpty(email))
            {
                accounts = accounts.Where(x => x.Email.ToLower().Contains(email.ToLower()));
                ViewBag.emailSearch = email;
            }
            var result = accounts.ToPagedList(page, pageSize);
            return View(result);
        }

        // delete user
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var account = _context.Accounts.Where(x => x.AccountId == id).FirstOrDefault();
            var name = account.DisplayName;
            var answers = _context.Comments.ToList();
            var questions = _context.Questions.Where(x => x.AccountId == id).ToList();
            var totalQuestion = 0;
            foreach (var answer in answers)
            {
                if (answer.Reply == 0 && answer.AccountId == id)
                {
                    foreach (var answer_reply in answers)
                    {
                        if (answer_reply.Reply == answer.CommentId)
                        {
                            totalQuestion += 1;
                            _context.Comments.Remove(answer_reply);
                        }
                    }
                    totalQuestion += 1;
                    var updateQuestion = _context.Questions.Where(x => x.QuestionId == answer.QuestionId).FirstOrDefault();
                    updateQuestion.TotalQuestion = updateQuestion.TotalQuestion - totalQuestion;
                    _context.Questions.Update(updateQuestion);
                    _context.Comments.Remove(answer);
                    totalQuestion = 0;

                }
                if (answer.Reply != 0 && answer.AccountId == id)
                {
                    var updateQuestion_2 = _context.Questions.Where(x => x.QuestionId == answer.QuestionId).FirstOrDefault();
                    updateQuestion_2.TotalQuestion = updateQuestion_2.TotalQuestion - 1;
                    _context.Questions.Update(updateQuestion_2);
                    _context.Comments.Remove(answer);
                }
            }

            if (questions.Count > 0)
            {
                foreach (var question in questions)
                {
                    var answersExistInQuestion = _context.Comments.Where(x=>x.QuestionId == question.QuestionId).ToList();
                    if (answersExistInQuestion.Count > 0)
                    {
                        foreach (var answer in answersExistInQuestion)
                        {
                                _context.Comments.Remove(answer);
                        }
                    }
                    _context.Questions.Remove(question);
                }
            }
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            TempData["message"] = "<div class='alert alert-success alert - dismissible' role='alert' style='margin-right: 31px; position: absolute; z-index: 1;right: 0;top: 85px;'>" +
                " Success Delete Account "+ name + " <button type = 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button> " + "</div>";
            return RedirectToAction(nameof(Account));
        }

        // lock user
        [Route("lock/{id}")]
        [Authorize]
        public async Task<IActionResult> AccountLock(int id)
        {
            var account = _context.Accounts.Where(x => x.AccountId == id).FirstOrDefault();
            account.Status = 4;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            TempData["message"] = "<div class='alert alert-success alert - dismissible' role='alert' style='margin-right: 31px; position: absolute; z-index: 1;right: 0;top: 85px;'>" +
               " Success Lock Account " + account.Email + " <button type = 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button> " + "</div>";
            return RedirectToAction(nameof(Account));
        }
        // Unlock user
        [Route("unlock/{id}")]
        [Authorize]
        public async Task<IActionResult> AccountUnLock(int id)
        {
            var account = _context.Accounts.Where(x => x.AccountId == id).FirstOrDefault();
            account.Status = 1;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            TempData["message"] = "<div class='alert alert-success alert - dismissible' role='alert' style='margin-right: 31px; position: absolute; z-index: 1;right: 0;top: 85px;'>" +
              " Success Unlock Account " + account.Email + " <button type = 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button> " + "</div>";
            return RedirectToAction(nameof(Account));
        }
    }
}
