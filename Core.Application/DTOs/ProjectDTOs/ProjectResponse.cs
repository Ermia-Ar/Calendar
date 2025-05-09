namespace Core.Application.DTOs.ProjectDTOs
{
    public class ProjectResponse
    {
        public string Id { get; set; }

        public string OwnerId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
