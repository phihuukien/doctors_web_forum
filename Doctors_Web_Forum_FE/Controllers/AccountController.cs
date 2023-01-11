using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Doctors_Web_Forum_FE.BusinessModels;
using Doctors_Web_Forum_FE.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using X.PagedList;

namespace Doctors_Web_Forum_FE.Controllers
{
    [Route("users")]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _context;
        public AccountController(DatabaseContext context)
        {
            _context = context;
        }
        // get all user and search
        [Route("")]
        public IActionResult Index(string userName, int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pageSize = 5;
            var account = _context.Accounts.Where(x => x.Status == 1 || x.Status == 3).AsQueryable();
            if (!string.IsNullOrEmpty(userName)) {
                account = account.Where(x => x.DisplayName.ToLower().Contains(userName.ToLower()));
                ViewBag.doctorName = userName;
            }
            var accounts = account.ToPagedList(page, pageSize) ;
            return View(accounts);
        }
       
    }
}