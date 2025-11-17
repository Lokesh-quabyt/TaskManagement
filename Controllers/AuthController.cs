using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Dtos;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Services;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Helper;
namespace TaskManagement.Controllers
{
    
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("api/auth")]
    public class AuthController :Controller 
    {
        private readonly ApplicationDbContext db;
        private readonly IJwtService jwtService;
        public AuthController(ApplicationDbContext db,IJwtService jwtService)
        {
            this.db = db;
            this.jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto request)
        {

            User user = new User
            {
                UserName = request.UserName,
                HashPassword = PasswordHasher.Hash(request.Password),
                Role = request.Role
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return Ok("User is Stored in the database");
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginDto login)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.UserName == login.UserName);

            if(user == null || !PasswordHasher.Verify(login.Password, user.HashPassword))
            {
                return Unauthorized();
            }

            var token = jwtService.GenerateToken(user);

            return Ok(new { token, user.Role });
        }
    }
}
