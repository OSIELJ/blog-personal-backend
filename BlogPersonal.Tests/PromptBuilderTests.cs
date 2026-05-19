using BlogPersonal.Services.IA;

namespace BlogPersonal.Tests;

public class PromptBuilderTests
{
    [Fact]
    public void BuildSummaryPrompt_ShouldContainContent()
    {
        var content = "This is a blog post about technology.";
        var prompt = PromptBuilder.BuildSummaryPrompt(content);
        Assert.Contains(content, prompt);
    }

    [Fact]
    public void BuildSummaryPrompt_ShouldContainJsonInstructions()
    {
        var content = "Some content";
        var prompt = PromptBuilder.BuildSummaryPrompt(content);
        Assert.Contains("JSON", prompt);
    }

    [Fact]
    public void BuildSummaryPrompt_ShouldContainSummaryKey()
    {
        var content = "Some content";
        var prompt = PromptBuilder.BuildSummaryPrompt(content);
        Assert.Contains("Summary", prompt);
    }

    [Fact]
    public void BuildSummaryPrompt_ShouldContainTagsKey()
    {
        var content = "Some content";
        var prompt = PromptBuilder.BuildSummaryPrompt(content);
        Assert.Contains("Tags", prompt);
    }

    [Fact]
    public void BuildSummaryPrompt_ShouldContainCategoryKey()
    {
        var content = "Some content";
        var prompt = PromptBuilder.BuildSummaryPrompt(content);
        Assert.Contains("Category", prompt);
    }
}