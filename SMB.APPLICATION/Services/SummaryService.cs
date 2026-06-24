using SMB.APPLICATION.DTOs.Summary;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.APPLICATION.Services;

public class SummaryService(ISummaryRepository repository) : ISummaryService
{
    public async Task<DashboardSummaryDto> GetSummaryByUserId(long userId)
    {
        var summary = await repository.GetSummaryByUserId(userId);
        return summary;
    }
}