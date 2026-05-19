using BlogPersonal.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BlogPersonal.Services.IA;

public class OpenAIService : IIAService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;
    private readonly string _model;
    private readonly ILogger<OpenAIService> _logger;

    public OpenAIService(IConfiguration configuration, ILogger<OpenAIService> logger)
    {
        _httpClient = new HttpClient();
        _apiKey = configuration["OpenAI:ApiKey"] ?? string.Empty;
        _baseUrl = configuration["OpenAI:BaseUrl"] ?? string.Empty;
        _model = configuration["OpenAI:Model"] ?? "gpt-3.5-turbo";
        _logger = logger;
    }

    public async Task<AiResultDto> GenerateSummaryAsync(string content)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            _logger.LogWarning("OpenAI API key not configured. Returning default result.");
            return GenerateDefaultResult(content);
        }

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            var prompt = PromptBuilder.BuildSummaryPrompt(content);

            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = "You are an assistant specialized in blog text analysis. Respond ONLY in valid JSON, without additional text." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 300,
                temperature = 0.5
            };

            var json = JsonSerializer.Serialize(requestBody);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_baseUrl, httpContent);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var responseObj = JsonSerializer.Deserialize<JsonElement>(responseJson);

            var messageContent = responseObj
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "{}";

            var result = JsonSerializer.Deserialize<AiResultDto>(messageContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? GenerateDefaultResult(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling AI API.");
            return GenerateDefaultResult(content);
        }
    }

    private static AiResultDto GenerateDefaultResult(string content)
    {
        var words = content.Split(' ').Take(5);
        return new AiResultDto
        {
            Summary = content.Length > 150 ? content[..150] + "..." : content,
            Tags = string.Join(", ", words),
            Category = "General"
        };
    }
}