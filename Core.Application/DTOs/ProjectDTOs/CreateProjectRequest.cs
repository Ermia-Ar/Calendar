namespace Core.Application.DTOs.ProjectDTOs
{
    public class CreateProjectRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string RequestMassage { get; set; }

        public string[] Members { get; set; }
    }
}
