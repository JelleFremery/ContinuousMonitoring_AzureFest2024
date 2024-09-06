using AdaptiveCards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace AlertToTeams.Teams;

public class TeamsMessage
{
    private readonly AdaptiveCard _card;

    public TeamsMessage(string title, string content, string investigationLink, string severity)
    {
        _card = new AdaptiveCard("1.4");
        _card.Body.Add(new AdaptiveTextBlock(severity switch
        {
            "Sev0" => $"Critical Error: {title}",
            "Sev1" => $"Error: {title}",
            "Sev2" => $"Warning: {title}",
            _ => title
        })
        {
            Color = severity switch
            {
                "Sev0" => AdaptiveTextColor.Attention,
                "Sev1" => AdaptiveTextColor.Attention,
                "Sev2" => AdaptiveTextColor.Accent,
                _ => AdaptiveTextColor.Default
            },
            Size = AdaptiveTextSize.ExtraLarge,
            Wrap = true
            

        });
        _card.Body.Add(new AdaptiveTextBlock(content)
        {
            Separator = true,
            Wrap = true
        });
        _card.Body.Add(new AdaptiveTextBlock($"Sent from `Azure Function` at **{DateTime.UtcNow}** (UTC).")
        {
            Size = AdaptiveTextSize.Small,
            Wrap = true
        });

        _card.Actions.Add(new AdaptiveOpenUrlAction
        {
            Title = "Powered by Betabit",
            Url = new Uri("https://www.betabit.nl"),
            IconUrl = "https://www.betabit.nl/static/assets/manifest/betabit/favicon-32x32.png"
        });
        _card.Actions.Add(new AdaptiveOpenUrlAction
        {
            Title = "View in App Insights",
            Url = new Uri(investigationLink)
        });

        _card.AdditionalProperties.Add("msteams", new { width = "Full" });
    }

    public string ToJson()
    {
        return $$"""{"type":"message","attachments":[{"contentType":"application/vnd.microsoft.card.adaptive","content":{{_card.ToJson()}}}]}""";
    }
}