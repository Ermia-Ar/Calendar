namespace Core.Domain.Helper
{
    public static class CalendarClaims
    {
        // Activity Controller
        public const string CreateActivity = "20254311";
        public const string CreateActivityForProject = "20254312";
        public const string CreateSubActivity = "20254313";
        public const string DeleteActivity = "20254314";
        public const string ExitingActivity = "20254315";
        public const string RemoveMemberOfActivity = "20254316";
        public const string UpdateActivity = "20254317";
        public const string CompleteActivity = "20254318";
        public const string GetAllUserActivity = "20254319";
        public const string GetMemberOfActivity = "20254320";

        // UserRequest Controller
        public const string SendRequest = "20254321";
        public const string AnswerRequest = "20254322";
        public const string RemoveRequest = "20254323";
        public const string GetRequestsReceived = "20254324";
        public const string GetRequestsResponse = "20254325";
        public const string GetUnAnsweredRequest = "20254326";
        public const string GetResponsesUserSent = "20254327";

        // Project Controller
        public const string CreateProject = "20254328";
        public const string RequestAddMemberToProject = "20254329";
        public const string GetMemberOfProject = "20254330";
        public const string GetActivitiesOfProject = "20254331";
        public const string GetAllUserProjects = "20254332";
        public const string ExitingProject = "20254333";
        public const string RemoveMemberOfProject = "20254334";
        public const string DeleteProject = "20254335";

        // Comment Controller
        public const string CreateComment = "20254336";
        public const string GetComments = "20254337";
        public const string UpdateComment = "20254338";
        public const string DeleteComment = "20254339";

        // Auth Controller
        public const string Register = "20254340";
        public const string Login = "20254341";
        public const string GetUserByUserName = "20254342";
        public const string GetAllUsers = "20254343";
    }
}
