using TaskManagement.Models;

namespace TaskManagement.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
