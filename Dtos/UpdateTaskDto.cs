namespace TaskManagement.Dtos
{
    public class UpdateTaskDto
    {

        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool IsCompleted { get; set; }


    }
}
