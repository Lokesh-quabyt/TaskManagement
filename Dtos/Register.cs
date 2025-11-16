namespace TaskManagement.Dtos
{
    public class Register
    {
        public required string UserName { get; set; }

        public required string Password { get; set; }

        public required string Role { get; set; }
    }
}
