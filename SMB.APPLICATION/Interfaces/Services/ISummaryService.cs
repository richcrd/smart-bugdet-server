using SMB.APPLICATION.DTOs.Summary;

namespace SMB.APPLICATION.Interfaces.Services;

public interface ISummaryService
{
    Task<DashboardSummaryDto> GetSummaryByUserId(long userId);
}