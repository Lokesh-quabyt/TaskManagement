using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public required string UserName { get; set; }

        public required string HashPassword { get; set; }

        public required string Role { get; set; }

    }
}
