using BlogPersonal.DTOs;

namespace BlogPersonal.Services.IA;

public interface IIAService
{
    Task<AiResultDto> GenerateSummaryAsync(string content);
}