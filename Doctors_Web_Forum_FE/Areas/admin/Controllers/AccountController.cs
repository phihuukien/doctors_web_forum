using Doctors_Web_Forum_FE.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
       
        [Route("delete/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            var account = _context.Accounts.Where(x => x.AccountId == id).FirstOrDefault();
            var name = account.DisplayName;
            var answers = _context.Comments.Where(x => x.AccountId == id).ToList();
            var questions = _context.Questions.Where(x => x.AccountId == id).ToList();
            var answersInQuestion = _context.Comments.ToList();
            if (answers.Count > 0)
            {
                var totalQuestion = 0;
                var questionId = 1;
                foreach (var answer in answers)
                {
                    questionId = answer.QuestionId;
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
                        var updateQuestion = _context.Questions.Where(x => x.QuestionId == questionId).FirstOrDefault();
                        updateQuestion.TotalQuestion = updateQuestion.TotalQuestion - totalQuestion;
                        _context.Questions.Update(updateQuestion);
                        _context.Comments.Remove(answer);
                        totalQuestion = 0;

                    }
                    if (answer.AccountId == id)
                    {
                        totalQuestion += 1;
                        var updateQuestion_2 = _context.Questions.Where(x => x.QuestionId == questionId).FirstOrDefault();
                        updateQuestion_2.TotalQuestion = updateQuestion_2.TotalQuestion - 1;
                        _context.Questions.Update(updateQuestion_2);
                        _context.Comments.Remove(answer);
                    }
                }
            }
            if (questions.Count > 0)
            {
                foreach (var question in questions)
                {
                    foreach (var answer in answersInQuestion)
                    {
                        if (question.QuestionId == answer.QuestionId)
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
                " Success Delete Account <button type = 'button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button> " + "</div>";
            return RedirectToAction(nameof(Account));
        }

        [Route("lock/{id}")]
        [Authorize]
        public async Task<IActionResult> AccountLock(int id)
        {
            var account = _context.Accounts.Where(x => x.AccountId == id).FirstOrDefault();
            account.Status = 4;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Account));
        }
        [Route("unlock/{id}")]
        [Authorize]
        public async Task<IActionResult> AccountUnLock(int id)
        { 
            var account = _context.Accounts.Where(x => x.AccountId == id).FirstOrDefault();
            account.Status = 1;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Account));
        }
    }
}
