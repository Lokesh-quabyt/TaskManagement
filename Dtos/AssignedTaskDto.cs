using Microsoft.AspNetCore.SignalR;

namespace TaskManagement.Dtos
{
    public class AssignedTaskDto
    {

        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool IsCompleted { get; set; }
        public required string UserName { get; set; }

    }
}
