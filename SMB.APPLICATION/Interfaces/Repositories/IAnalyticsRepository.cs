using SMB.APPLICATION.DTOs.Analytics;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface IAnalyticsRepository
{
    Task<AnalyticsResponseDto> GetAnalyticsByUserId(long userId);
}
