using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Controllers
{
    [Route("question")]
    public class QuestionController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View("Question");
        }
    }
}
