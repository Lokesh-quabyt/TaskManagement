using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration config;

        public JwtService(IConfiguration config)
        {
            this.config = config;
        }

        public string GenerateToken(User user)
        {

            var c = new[]
            {
                 new Claim(ClaimTypes.Name,user.UserName),
                 new Claim(ClaimTypes.Role,user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:key"]));

            var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                 issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                 claims: c,
                expires: DateTime.UtcNow.AddHours(2),
                 signingCredentials: cred
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }    
          
    
    }
}
