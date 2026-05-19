using BlogPersonal.Services.IA;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BlogPersonal.Tests;

public class OpenAIServiceTests
{
    private OpenAIService CreateService(string apiKey = "")
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "OpenAI:ApiKey", apiKey },
            { "OpenAI:BaseUrl", "https://api.openai.com/v1/chat/completions" },
            { "OpenAI:Model", "gpt-3.5-turbo" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var logger = new Mock<ILogger<OpenAIService>>();
        return new OpenAIService(configuration, logger.Object);
    }

    [Fact]
    public async Task GenerateSummaryAsync_WhenApiKeyIsEmpty_ShouldReturnDefaultResult()
    {
        var service = CreateService(apiKey: "");
        var result = await service.GenerateSummaryAsync("Some content about technology");

        Assert.NotNull(result);
        Assert.NotEmpty(result.Summary);
        Assert.NotEmpty(result.Tags);
        Assert.Equal("General", result.Category);
    }

    [Fact]
    public async Task GenerateSummaryAsync_WhenContentIsLong_ShouldTruncateSummary()
    {
        var service = CreateService(apiKey: "");
        var longContent = new string('a', 200);
        var result = await service.GenerateSummaryAsync(longContent);

        Assert.NotNull(result);
        Assert.True(result.Summary.Length <= 153);
    }

    [Fact]
    public async Task GenerateSummaryAsync_WhenContentIsShort_ShouldReturnFullContent()
    {
        var service = CreateService(apiKey: "");
        var shortContent = "Short content";
        var result = await service.GenerateSummaryAsync(shortContent);

        Assert.NotNull(result);
        Assert.Equal(shortContent, result.Summary);
    }

    [Fact]
    public async Task GenerateSummaryAsync_WhenApiKeyIsEmpty_ShouldReturnTagsFromContent()
    {
        var service = CreateService(apiKey: "");
        var content = "word1 word2 word3 word4 word5 word6";
        var result = await service.GenerateSummaryAsync(content);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Tags);
    }

    [Fact]
    public async Task GenerateSummaryAsync_WhenApiKeyIsInvalid_ShouldReturnDefaultResult()
    {
        var service = CreateService(apiKey: "invalid-key");
        var result = await service.GenerateSummaryAsync("Some content");

        Assert.NotNull(result);
        Assert.NotEmpty(result.Summary);
    }
}