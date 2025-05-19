namespace Core.Application.DTOs.UserRequestDTOs
{
    public class SendProjectRequest
    {
        public string ProjcetId { get; set; }

        public string[] Receivers { get; set; }

        public string? Message { get; set; }
    }
}
