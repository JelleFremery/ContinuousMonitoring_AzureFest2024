using System.Net.Http.Headers;
using AlertToTeams.Alert;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using AlertToTeams.Teams;

namespace AlertToTeams
{
    public class AlertFunctions
    {
        private readonly ILogger<AlertFunctions> _logger;
        private readonly TeamsSettings _settings;

        public AlertFunctions(ILogger<AlertFunctions> logger, TeamsSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        [Function(nameof(PostAlertToTeams))]
        public async Task<IActionResult> PostAlertToTeams([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var alert = JsonSerializer.Deserialize<AzureMonitorAlertMessage>(requestBody)!;

            var count = alert.data.alertContext.condition.allOf[0].metricValue;
            var time = alert.data.alertContext.condition.windowEndTime - alert.data.alertContext.condition.windowStartTime;
            var threshold = alert.data.alertContext.condition.allOf[0].threshold;
            var linkToFilteredSearchResultsUi = alert.data.alertContext.condition.allOf[0].linkToFilteredSearchResultsUI;
            var severity = alert.data.essentials.severity;
            TeamsMessage teamsBody;
            if (alert.data.essentials.alertRule == "Betabit-AzureFest-Exceptions")
            {
                teamsBody = new TeamsMessage("Too many temperature exceptions", 
                    $"Too many temperature exceptions: **{count}** times in {time}.", 
                    linkToFilteredSearchResultsUi, severity);
                _logger.LogError(
                    "Too many temperature exceptions: {count} times in {time}.", count, time);
            }
            else
            {
                teamsBody = new TeamsMessage("Page visit count Alert", 
                    $"Page was visited **{count}** times in {time}, which is more than the expected **{threshold}**.", 
                    linkToFilteredSearchResultsUi, severity);
                _logger.LogWarning(
                    "Page was visited {count} times in {time}, which is more than the expected {threshold}.", count,
                    time, threshold);
            }

            var client = new HttpClient
            {
                BaseAddress = _settings.WebhookUri
            };
            var json = teamsBody.ToJson();
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = await client.PostAsync(_settings.WebhookUri, byteContent).ConfigureAwait(false);

            if (result.IsSuccessStatusCode)
            {
                return new OkObjectResult(result.Content);
            }

            _logger.LogError("Failed to deliver webhook: {statusCode}: {body} ", result.StatusCode, await result.Content.ReadAsStringAsync());
            return new BadRequestResult();
        }
    }
}
