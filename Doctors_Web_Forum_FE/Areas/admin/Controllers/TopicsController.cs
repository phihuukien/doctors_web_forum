using Doctors_Web_Forum_FE.BusinessModels;
using Doctors_Web_Forum_FE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Areas.admin.Controllers
{
    [Area("admin")]
    [Route("admin/topics")]
    [Authorize]
    public class TopicsController : Controller
    {
        public readonly DatabaseContext _context;
        public TopicsController(DatabaseContext context)
        {
            _context = context;
        }

        // list topics 
        [Route("")]
        public IActionResult Index()
        {
            var topics = _context.Topics.OrderByDescending(x=>x.TopicId).ToList(); 
            return View(topics);
        }

        // Insert topic
        [Route("insert")]
        public async Task<IActionResult> Insert([FromForm] Topic topicForm)
        {
            if (topicForm ==null)
            {
                return BadRequest(new { message = "Faild Insert" });
            }
            var topic = _context.Topics.Where(x => x.TopicName == topicForm.TopicName).FirstOrDefault();
            if (topic != null)
            {
                return BadRequest(new { message = "Name already exists" });
            }
            topicForm.Status = true;
            _context.Topics.Add(topicForm);
            await _context.SaveChangesAsync();
            return Ok(new { message="Success Insert"});
        }

        // edit  topic = {id}
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var topic = _context.Topics.Where(x => x.TopicId == id).FirstOrDefault();

            return Ok(topic);
        }

        // update  topic = {id}
        [Route("update")]
        public async Task<IActionResult> Update([FromForm ] Topic topic)
        {
            _context.Topics.Update(topic);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Success Update" });
        }
    }
}
