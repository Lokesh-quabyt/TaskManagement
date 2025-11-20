using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagement.Data;
using TaskManagement.Dtos;
using TaskManagement.Helper;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {

        private readonly ApplicationDbContext db;

        public UserController(ApplicationDbContext db)
        {
            this.db = db;
        }

       [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(db.Users.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return NotFound("User not found");

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(AddUserDto newUser)
        {
          
            User nUser = new User
            {
                UserName = newUser.UserName,
                HashPassword = PasswordHasher.Hash(newUser.Password),
                Role = newUser.Role
            };

            await db.Users.AddAsync(nUser);
            await db.SaveChangesAsync();

            return Ok(nUser);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id,UpdateUser updatedUser)
        {
            var user = db.Users.Find(id);
            if (user == null) return NotFound("User not found");

           user.UserName = updatedUser.UserName;
            user.HashPassword = PasswordHasher.Hash(updatedUser.Password);
           user.Role = updatedUser.Role;

           await db.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return NotFound("User not found");

            db.Users.Remove(user);
            db.SaveChanges();
            return Ok("User deleted");
        }
    }
}
