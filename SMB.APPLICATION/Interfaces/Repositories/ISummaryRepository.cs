using SMB.APPLICATION.DTOs.Summary;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface ISummaryRepository
{
    Task<DashboardSummaryDto> GetSummaryByUserId(long userId);
}