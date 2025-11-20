using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Dtos;
using TaskManagement.Models;



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

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDto task)
        {
            var username = User.Identity.Name;
            var u = db.Users.First(u => u.UserName == username);

            TaskItem newTask = new TaskItem
            {
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                UserId = u.Id,
                user = u
            };

            await  db.Tasks.AddAsync(newTask);
            await db.SaveChangesAsync();

            return Ok(task);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMyTask(int id, UpdateTaskDto updatedTask)
        {
            var username = User.Identity.Name;
            var user = db.Users.First(u => u.UserName == username);

            var task = db.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == user.Id);
            if (task == null) return Unauthorized("You cannot edit this task");

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.IsCompleted = updatedTask.IsCompleted;

            db.SaveChanges();
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMyTask(int id)
        {
            var username = User.Identity.Name;
            var user = db.Users.First(u => u.UserName == username);

            var task = db.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == user.Id);
            if (task == null) return Unauthorized("You cannot delete this task");

            db.Tasks.Remove(task);
            db.SaveChanges();

            return Ok("Task deleted");
        }

    }
}
