using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Dtos;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Controllers
{
    
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("api/auth")]
    public class AuthController :Controller 
    {
        private readonly ApplicationDbContext db;
        public AuthController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto request)
        {

            User user = new User
            {
                UserName = request.UserName,
                HashPassword = request.Password,
                Role = request.Role
            };

            await db.users.AddAsync(user);
            await db.SaveChangesAsync();

            return Ok("User is Stored in the database");
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginDto login)
        {
            var user = await db.users.FirstOrDefaultAsync(x => x.UserName == login.UserName);

            if(user == null || !PasswordHandler.verify(login.Password, user.HashPassword))
            {
                return Unauthorized();
            }

            var token = jwt.generateToken(user);

            return Ok({ token,user.Role});
        }
    }
}
