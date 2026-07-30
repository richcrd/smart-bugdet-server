using SMB.APPLICATION.DTOs.Analytics;

namespace SMB.APPLICATION.Interfaces.Services;

public interface IAnalyticsService
{
    Task<AnalyticsResponseDto> GetAnalyticsByUserId(long userId);
}
