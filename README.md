# AzureFest2024

Achieving Continuous Monitoring - Jelle Fremery - 11 September 2024
With great help from esteemed colleague Daniel te Winkel.

## Syllabus

In this session, I will demonstrate how to `generate and store monitoring and analytics data` for your `containerized application in Azure`, `turn that data into actionable information` , and `act on that information by setting up a durable integration with Azure DevOps` to inform your planning and optimize the value of new development.

By now, the DevOps lifecycle is a well-known and reasonably understood theoretical framework. Your team should strive to achieve the `"7Cs"` whenever feasible. The DevOps mindset has been applied to other fields, resulting in DevSecOps, DataOps, AIOps, and MLOps, collectively known as XOps.

So, the theory is sound. But to actually accomplish this in practice, with an actual application, is another challenge altogether. Based on a real DevOps implementation journey, I will share how I implemented the "C" of the 7Cs that is often lacking even in mature DevOps cycles: `Continuous Monitoring`. I will also disclose where I went wrong and how to further improve upon what I delivered in the end.

What can you expect? As you are probably aware, Azure has plenty of services that can help us set up and professionalize the monitoring process. As an obedient DevOps disciple, I started with a containerized application using Azure Container Apps. I will show how to gather data using `Azure Monitor` and `Azure Sentinel`, set effective alerts based on pre-determined, smart triggers to turn that data into actionable information, and use `Action Groups and Functions to create and update items on an Azure DevOps board` (or other ALM tooling). Adding more customization using `KQL for custom queries` of the telemetry and creating `dashboards in Azure DevOps` that connect back to all that monitoring using `Power BI` will also be on the program. And finally, the cherry on top: `integrating with Microsoft Outlook and Microsoft Teams` to fully automate the jump from monitoring and operations to planning.

After this session, you will have plenty of inspiration and examples of using monitoring, analytics, and telemetry to build higher-quality and more secure cloud applications. If you want to ensure you can keep building quality software, make sure you join me and learn how to automate as much of the monitoring process as possible!

## The talk

- [The talk](https://www.azurefest.nl/2024/session/achieving-continuous-monitoring-by-integrating-azure-and-azure-devops)
- [The repo](https://github.com/JelleFremery/AzureFest2024.git)

## Core elements

- Generate and store monitoring and analytics data
  - App insights
  - Log Analytics workspace
  - Azure Monitor
  - Azure Sentinel
  - Auditing on all resources.
- Turn log data into actionable information via Continuous Monitoring
- Act on that information in a structured manner by setting up a durable integration with Azure DevOps
- Containerized Application in Azure 
- [7 Cs of DevOps](https://www.geeksforgeeks.org/devops-lifecycle/)
- Action Groups and Functions to create and update items on an Azure DevOps
- KQL for custom queries
- Option to set up dashboards in Power BI to display in Azure DevOps
- Integrating with Microsoft Outlook and Microsoft Teams
  - Default / custom (especially teams)

![overview](Overview.drawio.png)

### Resources used during talk
| Resource | Purpose |
| --- | --- |
| Resource Group | Logical container to group (almost) all revant resoruces. |
| Log Analytics workspace | Gather App Insights |
| App Insights | Collect logs from the container apps. |
| Azure Container Registry | Register apps |
| Security Insights | Microsoft.OperationsManagement/solutions |
| Container application environment. | Hosts the container apps. |
| User assigned Identity | For instance, used to allow the container app to pull from the ACR. |
| Cache (Azure container app with Redis cache) | To cache the requests to the Weather API. |
| ApiService (Azure container app with API) | To provide (generated) weather information. |
| WebFrontend (Azure container app with Blazor web app) | Display weather information. |
| AlertToTeams (Azure container app with Azure function) | Translates the common alert message to a dynamic message for Teams and send it to teams. Changes the message depending on the incoming alert type. Adds buttons to the message to make it actionable. |
| Action Group (ToTeams) | Action group to send alerts to Teams. Also sends alerts to Request Inspector to be able to inspect the messages. |
| ActionGroup (ToMail) | Action group to send alerts to e-mail. |
| Weather Alert  | Alert that, once every 5 minutes, checks there have not been more than 20 requests to the app in that time span. Alert uses the two action groups to send the alert to the right places. |
| Exceptions Alert | Alert that, once every minute, checks there have not been any 'too cold' exceptions in the API in that time span. Alert uses the two action groups to send the alert to the right places. |

### Containerized application in Azure

- [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Azure container apps](https://azure.microsoft.com/en-us/products/container-apps)
- [Deploy apps to Azure Container Apps easily with .NET Aspire](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deploy-apps-to-azure-container-apps-easily-with-net-aspire/ba-p/4032711)
- [Deploy a .NET Aspire project to Azure Container Apps](https://learn.microsoft.com/en-us/dotnet/aspire/deployment/azure/aca-deployment?tabs=visual-studio%2Clinux%2Cpowershell&pivots=azure-azd)

## Related Betatalks

- [Betatalks #97 - Microsoft Azure Sentinel](https://www.betabit.nl/betatalks-videos/betatalks-97-microsoft-azure-sentinel)
- [Betatalks #91 - Early impressions of .NET Aspire: Cloud-first development](https://www.betabit.nl/betatalks-videos/betatalks-91-early-impressions-of-net-aspire-cloud-first-development)

## Slightly related Betatalks

- [Betatalks #95 - .NET Essentials: Logging in .NET (Part 1)](https://www.betabit.nl/betatalks-videos/betatalks-95-net-essentials-logging-in-net-part-1)
- [Betatalks #96 - .NET Essentials: Logging in .NET (Part 2)](https://www.betabit.nl/betatalks-videos/betatalks-96-net-essentials-logging-in-net-part-2)

## Demo project

The example is taken from  [Deploy apps to Azure Container Apps easily with .NET Aspire](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deploy-apps-to-azure-container-apps-easily-with-net-aspire/ba-p/4032711) to create an Aspire app. The project is added to the `DemoApp` folder of this solution.

Install:

- [Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling)
- [Aspir8](https://github.com/prom3theu5/aspirational-manifests)
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/)

To build the docker images, fill out missing config, store images in Azure Container Registry and deploy them as Azure Container apps, run the `DeployTo-Azure.ps1` script in the root of this repository.
