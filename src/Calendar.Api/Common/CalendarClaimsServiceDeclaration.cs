namespace Calendar.Api.Common
{
    public static class CalendarClaimsServiceDeclaration
    {
        // 🟦 Activities Controller
        public const string CreateActivity = "200003";
        public const string CreateSubActivity = "200036";
        public const string SendJoinRequest = "200004";
        public const string GetAllActivities = "200005";
        public const string GetActivityMembers = "200006";
        public const string GetActivityById = "200007";
        public const string UpdateActivity = "200008";
        public const string CompleteActivity = "200009";
        public const string ChangeNotificationTime = "200010";
        public const string ChangeStartDate = "200011";
        public const string DeleteActivity = "200012";
        public const string LeaveActivity = "200013";
        public const string RemoveActivityMember = "200014";

        // 🟩 Auth Controller
        public const string Login = "200015";
        public const string GetAllUsers = "200016";

        // 🟧 Comments Controller
        public const string AddComment = "200017";
        public const string UpdateComment = "200018";
        public const string DeleteComment = "200019";
        public const string GetCommentById = "200020";
        public const string GetAllComments = "200021";

        // 🟥 Projects Controller
        public const string CreateProject = "200002";
        public const string AddMemberToProject = "200023";
        public const string AddActivityToProject = "200024";
        public const string GetProjectActivities = "200025";
        public const string GetProjectMembers = "200026";
        public const string GetAllProjects = "200027";
        public const string GetProjectById = "200028";
        public const string LeaveProject = "200029";
        public const string RemoveProjectMember = "200030";
        public const string DeleteProject = "200031";

        // 🟪 Requests Controller
        public const string AnswerRequest = "200032";
        public const string DeleteRequest = "200033";
        public const string GetRequestById = "200034";
        public const string GetAllRequests = "200035";
    }
}
