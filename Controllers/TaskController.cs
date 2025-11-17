using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Data;



namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TaskController : ControllerBase
    {

        private readonly ApplicationDbContext db;

        public TaskController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet("my")]
        public IActionResult GetMyTasks()
        {
            var username = User.Identity.Name;
            var user = db.Users.First(u => u.UserName == username);

            var tasks = db.Tasks.Where(t => t.UserId == user.Id).ToList();

            return Ok(tasks);
        }
    }
}
