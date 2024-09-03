using System.Text.Json.Serialization;

public class TeamsMessage
{
    public TeamsMessage(string content)
    {
        attachments[0].content.body[1].text = content;
        attachments[0].content.body[2].text = $"Sent from `Azure Function` at **{DateTime.Now}**.";
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
            text = "Page visit count Alert",
            color = "Accent",
            size = "ExtraLarge"
        },
        new TextBlock
        {
            separator = true
        },
        new TextBlock()
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
}
