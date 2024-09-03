using AlertToTeams.Alert;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

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
            var teamsBody = new TeamsMessage($"Page was visited **{count}** times in {time}, which is more than the expected **{threshold}**.");

            _logger.LogWarning("Page was visited {count} times in {time}, which is more than the expected {threshold}.", count, time, threshold);
            var client = new HttpClient
            {
                BaseAddress = _settings.WebhookUri
            };
            var result = await client.PostAsJsonAsync(_settings.WebhookUri, teamsBody).ConfigureAwait(false);

            if (result.IsSuccessStatusCode)
            {
                return new OkObjectResult(result.Content);
            }

            _logger.LogError("Failed to deliver webhook: {statusCode}: {body} ", result.StatusCode, await result.Content.ReadAsStringAsync());
            return new BadRequestResult();
        }
    }
}
