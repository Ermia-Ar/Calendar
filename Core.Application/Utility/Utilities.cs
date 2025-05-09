using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Entity;
using System.Text.RegularExpressions;

namespace Core.Application.Utility
{
    public static class Utilities
    {
        public static bool IsEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        public static ActivityResponse ConvertToActivityResponse(Activity activity) 
        {
            return new ActivityResponse
            {
                Category = activity.Category,
                Date = activity.Date,
                Description = activity.Description,
                Id = activity.Id,
                IsCompleted = activity.IsCompleted,
                Title = activity.Title,
            };
        }
    }
}
