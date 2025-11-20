namespace TaskManagement.Dtos
{
    public class AddUserDto
    {

        public required string UserName { get; set; }

        public required string Password { get; set; }

        public required string Role { get; set; }
    }
}
