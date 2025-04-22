namespace Core.Application.DTOs.UserRequestDTOs
{
    public class SendRequest
    {
        public string ActivityId { get; set; }

        public string Receiver { get; set; }

        public string? Message { get; set; }
    }
}
