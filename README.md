# Syskit Point to ServiceNow and Jira Integration

This repository contains Azure Functions for integrating Syskit Point with ServiceNow and Jira. The functions process webhook events from Syskit Point and automatically create tickets in ServiceNow and Jira.

## Prerequisites

- **Azure Subscription**: Required to deploy Azure Functions.
- **ServiceNow and/or Jira Accounts**: With the necessary API access.
- **Visual Studio 2022 or later**: For local development and deployment.
- **.NET Core SDK**: Version 3.1 or later.
- **Azure Functions Tools**: Installed in Visual Studio.
- **Syskit Point** instance

## Setup Instructions

### 1. Clone the Repository

Clone the repository to your local machine:

git clone https://github.com/SysKitTeam/point-functions.git
cd point-functions

### 2. Configure Syskit Point
Follow the Syskit Point documentation to 
1. [configure Azure AD app](https://docs.syskit.com/point/integrations/syskit-point-api) that will be used to work with your API
2. [Obtain the Syskit Point secret](https://docs.syskit.com/point/integrations/syskit-point-api).
3. Once you have obtained the secret key, modify **Config.cs** with the value obtained.
- **`SyskitSecretKey`**: Your secret key for validating Syskit Point webhook signatures.

### 3. Configure the Connection to ServiceNow and Jira

Please consult [ServiceNow](https://docs.servicenow.com/bundle/washingtondc-api-reference/page/build/applications/concept/api-rest.html) and [Jira](https://developer.atlassian.com/cloud/jira/platform/rest/v3/intro/) documentation for more details on how to obtain these keys and setup the system. Optionally you can also change the code to use alternative authentication method. For security, you can store sensitive information in environment variables instead of hardcoding them, change the code as you see fit.

Replace the placeholder values with your actual credentials and settings:

If you intend to use ServiceNow:
- **`ServiceNowUrl`**: The URL of your ServiceNow instance.
- **`ServiceNowUsername`** and **`ServiceNowPassword`**: Your ServiceNow API credentials.

If you intend to use Jira:
- **`JiraUrl`**: This is the endpoint where issues will be created. Ensure that the URL points to your Jira instance.
- **`JiraEmail`**: The email address linked to your Jira account, which will be used in the API authentication process.
- **`JiraApiToken`**: A secure token generated in Jira, used instead of your Jira password for API authentication. You can generate it from your Jira account settings.
- **`JiraProjectKey`**: The project key in Jira where the issues will be created. This is typically a short code that identifies your project.
- **`JiraIssueType`**: The type of issue you want to create, such as `"Task"`, `"Bug"`, or `"Story"`. This defines the category of the issue in Jira.

### 4. Run Locally
You can test the functions locally before deploying:

1. Open the Solution in Visual Studio.
2. Restore NuGet Packages:
 - Right-click on the solution in Solution Explorer and select Restore NuGet Packages.
3. Run the Functions:
 - Press F5 to run the project locally. The Azure Functions runtime will start, and local endpoints will be available for testing.

### 5. Deploy to Azure
Publish from Visual Studio
1. Right-click on the project in Solution Explorer.
2. Select Publish.
3. Follow the prompts to deploy to your Azure subscription

Please note: These functions should be deployed as HTTP triggered.

### 6. Verify Deployment
Obtain the URL of the deployed Azure function(s) and [register them as webhooks in Syskit Point](https://docs.syskit.com/point/integrations/webhooks#registering-a-webhook-endpoint-with-syskit-point-api).

### 7. Verify Deployment
Once deployed, verify that the functions are working by sending test webhook events from Syskit Point. Monitor the logs in the Azure portal to ensure tickets are being created in ServiceNow and Jira. Set up Application Insights to monitor the performance and logging of your Azure Functions.

### 8. Contributing
Please feel free to submit issues or pull requests to improve the project.

### 9. License
This project is licensed under the MIT License.
