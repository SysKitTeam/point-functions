using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Syskit.Point.Functions;
using System.Net.Http;
using System.Threading.Tasks;

public static class SyskitPointToServiceNow
{
    [FunctionName("SyskitPointToServiceNow")]
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
            HttpResponseMessage response = await CommonHelpers.CreateServiceNowIncident(
                client,
                summary,
                description,
                "Provisioning",
                "General",
                "3");

            if (response.IsSuccessStatusCode)
            {
                log.LogInformation("ServiceNow incident successfully created.");
                return new OkObjectResult("ServiceNow incident created successfully");
            }
            else
            {
                log.LogError($"Failed to create ServiceNow incident. Status code: {response.StatusCode}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
