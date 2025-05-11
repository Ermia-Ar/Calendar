using System.ComponentModel.DataAnnotations;

namespace Core.Application.DTOs.UserRequestDTOs
{
    public class SendActivityRequest
    {
        public string ActivityId { get; set; }

        public string[] Receivers { get; set; }

        public bool IsGuest { get; set; }

        public string? Message { get; set; }
    }
}

