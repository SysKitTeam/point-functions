# Syskit Point to ServiceNow and Jira Integration

This repository contains Azure Functions for integrating Syskit Point with ServiceNow and Jira. The functions process webhook events from Syskit Point and automatically create tickets in ServiceNow and Jira.

## Prerequisites

- **Azure Subscription**: Required to deploy Azure Functions.
- **ServiceNow and/or Jira Accounts**: With the necessary API access.
- **Visual Studio 2022 or later**: For local development and deployment.
- **.NET Core SDK**: Version 3.1 or later.
- **Azure Functions Tools**: Installed in Visual Studio.

## Setup Instructions

### 1. Clone the Repository

Clone the repository to your local machine:

git clone https://github.com/SysKitTeam/point-functions.git
cd point-functions

### 2. Configure Syskit Point
Follow the Syskit Point documentation to [configure Azure AD app](https://docs.syskit.com/point/integrations/syskit-point-api) that will be used to work with your API and then [obtain the Syskit Point secret](https://docs.syskit.com/point/integrations/syskit-point-api).

Once you have obtained the secret key, modify Config.cs with the value obtained.
- **`SyskitSecretKey`**: Your secret key for validating Syskit Point webhook signatures.

### 3. Configure the Connection to ServiceNow and Jira

Please consult [ServiceNow](https://docs.servicenow.com/bundle/washingtondc-api-reference/page/build/applications/concept/api-rest.html) and [Jira](https://developer.atlassian.com/cloud/jira/platform/rest/v3/intro/) documentation for more details on how to obtain these keys and setup the system. Optionally you can also change the code to use alternative authentication method. Replace the placeholder values with your actual credentials and settings:

If you intend to use ServiceNow:
- **`ServiceNowUrl`**: The URL of your ServiceNow instance.
- **`ServiceNowUsername`** and **`ServiceNowPassword`**: Your ServiceNow API credentials.

If you intend to use Jira:
- **`JiraUrl`**: This is the endpoint where issues will be created. Ensure that the URL points to your Jira instance.
- **`JiraEmail`**: The email address linked to your Jira account, which will be used in the API authentication process.
- **`JiraApiToken`**: A secure token generated in Jira, used instead of your Jira password for API authentication. You can generate it from your Jira account settings.
- **`JiraProjectKey`**: The project key in Jira where the issues will be created. This is typically a short code that identifies your project.
- **`JiraIssueType`**: The type of issue you want to create, such as `"Task"`, `"Bug"`, or `"Story"`. This defines the category of the issue in Jira.

