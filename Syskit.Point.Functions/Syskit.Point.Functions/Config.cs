namespace Syskit.Point.Functions
{
    public static class Config
    {
        public static readonly string SyskitSecretKey = "your-syskit-secret-key";

        public static readonly string ServiceNowUrl = "https://your-instance.service-now.com/api/now/table/incident";
        public static readonly string ServiceNowUsername = "your-servicenow-username";
        public static readonly string ServiceNowPassword = "your-servicenow-password";
        public static readonly string ServiceNowCallerId = "your-callerid";

        public static readonly string JiraUrl = "https://your-instance.atlassian.net/rest/api/2/issue";
        public static readonly string JiraEmail = "your-jira-username";
        public static readonly string JiraApiToken = "your-jira-api-token";
        public static readonly string JiraProjectKey = "your-jira-project-key";
        public static readonly string JiraIssueType = "Task";
    }

}
