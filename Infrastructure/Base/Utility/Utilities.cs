using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Entity;
using Core.Domain.Enum;
using System.Text.RegularExpressions;

namespace Infrastructure.Base.Utility
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
                StartDate = activity.StartDate,
                Description = activity.Description,
                Id = activity.Id,
                IsCompleted = activity.IsCompleted,
                Title = activity.Title,
            };
        }
    }

}

//namespace Core.Services
//{
//    public class RecurrenceService
//    {
//        public IEnumerable<Activity> GenerateOccurrences(IEnumerable<Activity> activities, DateTime start, DateTime end)
//        {
//            var result = new List<Activity>();

//            foreach (var activity in activities)
//            {
//                // اگر تکرار نداره، خود Activity رو اضافه کن
//                if (activity.RecurrenceType == RecurrenceType.None)
//                {
//                    if (activity.Date >= start && activity.Date <= end)
//                    {
//                        result.Add(activity);
//                    }
//                    continue;
//                }

//                // تولید نمونه‌های تکراری
//                var occurrences = GenerateOccurrencesForActivity(activity, start, end);
//                result.AddRange(occurrences);
//            }

//            return result.OrderBy(a => a.Date).ToList();
//        }

//        private IEnumerable<Activity> GenerateOccurrencesForActivity(Activity activity, DateTime start, DateTime end)
//        {
//            var occurrences = new List<Activity>();
//            var currentDate = activity.Date.Date;
//            var recurrenceEnd = activity.RecurrenceEndDate ?? end;

//            while (currentDate <= recurrenceEnd)
//            {
//                if (currentDate >= start && IsValidOccurrence(activity, currentDate))
//                {
//                    var occurrence = new Activity
//                    {
//                        Id = Guid.NewGuid().ToString(), 
//                        ProjectId = activity.ProjectId,
//                        UserId = activity.UserId,
//                        User = activity.User, 
//                        Project = activity.Project,
//                        Title = activity.Title,
//                        Description = activity.Description,
//                        Date = currentDate + activity.Date.TimeOfDay,
//                        Duration = activity.Duration,
//                        CreatedDate = activity.CreatedDate,
//                        RecurrenceType = RecurrenceType.None, 
//                        IsCompleted = activity.IsCompleted
//                    };
//                    occurrences.Add(occurrence);
//                }

//                switch (activity.RecurrenceType)
//                {
//                    case RecurrenceType.Daily:
//                        currentDate = currentDate.AddDays(activity.RecurrenceInterval ?? 1);
//                        break;
//                    case RecurrenceType.Weekly:
//                        currentDate = currentDate.AddDays(1);
//                        break;
//                    case RecurrenceType.Monthly:
//                        currentDate = currentDate.AddMonths(activity.RecurrenceInterval ?? 1);
//                        break;
//                }
//            }

//            return occurrences;
//        }

//        private bool IsValidOccurrence(Activity activity, DateTime date)
//        {
//            if (activity.RecurrenceType != RecurrenceType.Weekly)
//                return true;

//            var daysOfWeek = activity.RecurrenceDaysOfWeek?.Split(',') ?? Array.Empty<string>();
//            var dayOfWeek = date.DayOfWeek.ToString().Substring(0, 3); 
//            return daysOfWeek.Length == 0 || daysOfWeek.Contains(dayOfWeek);
//        }
//    }
//}