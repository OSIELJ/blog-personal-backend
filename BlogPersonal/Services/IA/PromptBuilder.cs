namespace BlogPersonal.Services.IA;

public static class PromptBuilder
{
    public static string BuildSummaryPrompt(string content)
    {
        return $"""
            Analyze the text below and return a JSON with exactly these keys:
            - "Summary": a short summary of the text (maximum 2 sentences)
            - "Tags": up to 5 keywords separated by comma
            - "Category": a single category that best classifies the text

            Text:
            {content}

            Respond ONLY with the JSON, no explanations.
            """;
    }
}