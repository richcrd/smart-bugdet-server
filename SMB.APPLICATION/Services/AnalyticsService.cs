using SMB.APPLICATION.DTOs.Analytics;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.APPLICATION.Services;

public class AnalyticsService(IAnalyticsRepository repository) : IAnalyticsService
{
    public async Task<AnalyticsResponseDto> GetAnalyticsByUserId(long userId)
    {
        return await repository.GetAnalyticsByUserId(userId);
    }
}
