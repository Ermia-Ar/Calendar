namespace Core.Domain.Exceptions
{
    public static class CalendarClaimsServiceDeclaration
    {
        // Activity Controller
        public const string CreateActivity = "200002";
        public const string CreateActivityForProject = "200003";
        public const string CreateSubActivity = "200004";
        public const string DeleteActivity = "200005";
        public const string ExitingActivity = "200006";
        public const string RemoveMemberOfActivity = "200007";
        public const string UpdateActivity = "200008";
        public const string CompleteActivity = "200009";
        public const string GetAllUserActivity = "200010";
        public const string GetMemberOfActivity = "200012";

        // UserRequest Controller
        public const string SendActivityRequest = "200013";
        public const string SendProjectRequest = "200021";
        public const string AnswerRequest = "200014";
        public const string RemoveRequest = "200015";
        public const string GetRequestsReceived = "200016";
        public const string GetRequestsResponse = "200017";
        public const string GetUnAnsweredRequest = "200018";
        public const string GetResponsesUserSent = "200019";

        // Project Controller
        public const string CreateProject = "200020";
        public const string GetMemberOfProject = "200022";
        public const string GetActivitiesOfProject = "200023";
        public const string GetAllUserProjects = "200024";
        public const string ExitingProject = "200025";
        public const string RemoveMemberOfProject = "200026";
        public const string DeleteProject = "200027";

        // Comment Controller
        public const string CreateComment = "200028";
        public const string GetComments = "200029";
        public const string UpdateComment = "200030";
        public const string DeleteComment = "200031";

        // Auth Controller
        public const string Register = "200032";
        public const string Login = "200034";
        public const string GetUserByUserName = "200035";
        public const string GetAllUsers = "200036";
    }
}
