using System;

namespace Syskit.Point.Functions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System.IO;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public static class CommonHelpers
    {
        private static string generateSignature(string content, string authKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(authKey);
            var payloadBytes = Encoding.UTF8.GetBytes(content);

            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(payloadBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static bool ValidateSignature(string secretKey, string requestBody, string receivedSignature)
        {
            return generateSignature(requestBody, secretKey) == receivedSignature;
        }


        public static async Task<(string Summary, string Description)> ProcessWebhook(HttpRequest req, ILogger log)
        {
            log.LogInformation("Syskit Point webhook triggered function.");

            // Read the request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation($"Received payload: {requestBody}");

            // Retrieve the signature from headers
            string receivedSignature = req.Headers["Signature"];

            // Validate the signature
            if (!ValidateSignature(Config.SyskitSecretKey, requestBody, receivedSignature))
            {
                log.LogError($"Signature validation failed. Signature {receivedSignature}");

                // TODO: Fix
                //return (null, null); // Handle this appropriately in the calling function
            }

            // Deserialize the payload
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string name = data?.content?.name;
            string type = data?.content?.type;
            string url = data?.content?.url;
            string microsoftId = data?.content?.microsoftId;

            // Create summary and description
            string summary = $"Provisioning Completed: {name} ({type})";
            string description = $"The provisioning process for {type} named {name} has completed successfully. Access it at {url}. Microsoft ID: {microsoftId}.";

            return (summary, description);
        }


        public static async Task<HttpResponseMessage> CreateServiceNowIncident(
            HttpClient client,
            string shortDescription,
            string description,
            string category,
            string subcategory,
            string priority)
        {
            var jsonContent = new
            {
                short_description = shortDescription,
                description = description,
                caller_id = Config.ServiceNowCallerId,
                category = category,
                subcategory = subcategory,
                priority = priority
            };

            string json = JsonConvert.SerializeObject(jsonContent);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{Config.ServiceNowUsername}:{Config.ServiceNowPassword}")));

            return await client.PostAsync(Config.ServiceNowUrl, httpContent);
        }

        public static async Task<HttpResponseMessage> CreateJiraIssue(
            HttpClient client,
            string summary,
            string description)
        {
            var jsonContent = new
            {
                fields = new
                {
                    project = new { key = Config.JiraProjectKey },
                    summary = summary,
                    description = description,
                    issuetype = new { name = Config.JiraIssueType }
                }
            };

            string json = JsonConvert.SerializeObject(jsonContent);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("Accept", "application/json");

            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Config.JiraEmail}:{Config.JiraApiToken}"));
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");

            HttpResponseMessage response = await client.PostAsync(Config.JiraUrl, httpContent);

            return await client.PostAsync(Config.JiraUrl, httpContent);
        }
    }

}
