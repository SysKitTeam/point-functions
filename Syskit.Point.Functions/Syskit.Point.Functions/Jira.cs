using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Syskit.Point.Functions;
using System.Net.Http;
using System.Threading.Tasks;

public static class SyskitPointToJira
{
    [FunctionName("SyskitPointToJira")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        var (summary, description) = await CommonHelpers.ProcessWebhook(req, log);

        if (summary == null || description == null)
        {
            return new UnauthorizedResult(); // Handle signature validation failure
        }

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await CommonHelpers.CreateJiraIssue(
                client,
                summary,
                description);

            if (response.IsSuccessStatusCode)
            {
                log.LogInformation("Jira issue successfully created.");
                return new OkObjectResult("Jira issue created successfully");
            }
            else
            {
                log.LogError($"Failed to create Jira issue. Status code: {response.StatusCode}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
