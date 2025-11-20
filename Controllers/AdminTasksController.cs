using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Data;
using TaskManagement.Dtos;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{

    [ApiController]
    [Route("api/admin/tasks")]
    [Authorize(Roles = "Admin")]
    public class AdminTasksController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AdminTasksController(ApplicationDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public IActionResult GetAllTasks()
        {
            return Ok(_context.Tasks.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetTaskById(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null) return NotFound("Task not found");

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(AssignedTaskDto task)
        {

            User u = _context.Users.First(u => u.UserName == task.UserName);

            TaskItem newTask = new TaskItem { 
                Title = task.Title,
                Description = task.Description,
                IsCompleted = false,
                UserId = u.Id,
                user = u
                };

           await  _context.Tasks.AddAsync(newTask);
           await  _context.SaveChangesAsync();
            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, AssignedTaskDto updatedTask)
        {
            var task = _context.Tasks.Find(id);
            if (task == null) return NotFound("Task not found");
            User u = _context.Users.First(u => u.UserName == updatedTask.UserName);

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.IsCompleted = updatedTask.IsCompleted;
            task.UserId = u.Id;
            task.user = u;

            await _context.SaveChangesAsync();
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null) return NotFound("Task not found");

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return Ok("Task deleted");
        }


    }
}
