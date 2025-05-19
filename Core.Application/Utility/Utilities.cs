using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.ProjectDTOs;
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

        public static ActivityResponse ConvertToActivityResponse(Activity? activity) 
        {
            return new ActivityResponse
            {
                Category = activity.Category,
                StartDate = activity.StartDate,
                Description = activity.Description,
                Id = activity.Id,
                IsCompleted = activity.IsCompleted,
                Title = activity.Title,
            };
        }

        public static ProjectResponse ConvertToProjectResponse(Project? activity)
        {
            return new ProjectResponse
            {
                CreatedDate = activity.CreatedDate,
                Description = activity.Description,
                Id = activity.Id,
                EndDate = activity.EndDate,
                OwnerId = activity.OwnerId,
                StartDate = activity.StartDate,
                Title = activity.Title,
                UpdateDate = activity.UpdateDate    
            };
        }
    }
}
