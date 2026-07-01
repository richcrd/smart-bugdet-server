using SMB.APPLICATION.DTOs.Wallet;

namespace SMB.APPLICATION.Interfaces.Services;

public interface IWalletService
{
    Task<List<WalletResponse>> GetAllByUserId(long userId);
    Task<WalletResponse> Create(long userId, CreateWalletRequest request);
    Task<WalletResponse> Update(long userId, long walletId, UpdateWalletRequest request);
    Task<WalletResponse> SetDefault(long userId, long walletId);
    Task Delete(long userId, long walletId);
}
