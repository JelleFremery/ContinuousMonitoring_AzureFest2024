using System.Text.Json.Serialization;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class TeamsMessage
{
    public TeamsMessage(string title, string content, string investigationLink)
    {
        attachments[0].content.body[0].text = title;
        attachments[0].content.body[1].text = content;
        attachments[0].content.body[2].text = $"Sent from `Azure Function` at **{DateTime.Now}**.";

        if (!string.IsNullOrEmpty(investigationLink))
        {
            attachments[0].content.actions.Add(new ButtonAction
            {
                title = "View in App Insights",
                url = investigationLink
            });
        }
    }

    public string type { get; set; } = "message";

    public Attachment[] attachments { get; set; } = [new Attachment()];
}


public class Attachment
{
    public string contentType { get; set; } = "application/vnd.microsoft.card.adaptive";
    public Content content { get; set; } = new Content();
}

public class Content
{
    [JsonPropertyName("$schema")]
    public string schema { get; set; } = "http://adaptivecards.io/schemas/adaptive-card.json";

    public string type { get; set; } = "AdaptiveCard";

    public string version { get; set; } = "1.0";

    public Msteams msteams { get; set; } = new Msteams();

    public TextBlock[] body { get; set; } = [
        new TextBlock
        {
            color = "Accent",
            size = "ExtraLarge"
        },
        new TextBlock
        {
            separator = true
        },
        new TextBlock()
    ];

    public IList<ButtonAction> actions { get; set; } =
    [
        new ButtonAction
        {
            title = "Powered by Betabit",
            url = "https://www.betabit.nl"
        }
    ];
}

public class Msteams
{
    public string width { get; set; } = "Full";
}

public abstract class BodyElement
{
    public abstract string type { get; set; }
}

public class TextBlock : BodyElement
{
    public override string type { get; set; } = nameof(TextBlock);
    public string text { get; set; }
    public string id { get; set; }
    public string spacing { get; set; }
    public string size { get; set; }
    public string weight { get; set; }
    public string color { get; set; }
    public bool separator { get; set; }
    public bool wrap { get; set; } = true;
}

public class ButtonAction
{
    public string role { get; set; } = "button";
    public string title { get; set; } = "Powered by Betabit";
    public string type { get; set; } = "Action.OpenUrl";
    public string url { get; set; } = "https://www.betabit.nl";
}