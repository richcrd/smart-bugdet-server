using SMB.APPLICATION.DTOs.Wallet;
using SMB.APPLICATION.Exceptions;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;
using SMB.DOMAIN.Constants;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Services;

public class WalletService(
    IWalletRepository walletRepository,
    ICatalogRepository catalogRepository,
    IUnitOfWork unitOfWork) : IWalletService
{
    public async Task<List<WalletResponse>> GetAllByUserId(long userId)
    {
        var wallets = await walletRepository.GetActiveByUserId(userId);
        return wallets.Select(Map).ToList();
    }

    public async Task<WalletResponse> Create(long userId, CreateWalletRequest request)
    {
        var name = request.Name.Trim();
        if (name.Length is < 3 or > 100)
        {
            throw new ValidationException("El nombre debe tener entre 3 y 100 caracteres");
        }

        var currency = await catalogRepository.GetCurrencyById(request.CurrencyId)
            ?? throw new ResourceNotFoundException("La moneda no existe");

        var status = await catalogRepository.GetStatusByCode(StatusCodes.Active)
            ?? throw new ResourceNotFoundException("El estado 'activo' no existe");

        var wallet = new Wallet
        {
            UserId = userId,
            CurrencyId = currency.Id,
            Name = name,
            InitialBalance = 0,
            CurrentBalance = 0,
            IsDefault = false,
            StatusId = status.Id,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        await walletRepository.AddAsync(wallet);
        await unitOfWork.SaveChangesAsync();

        var created = await walletRepository.GetById(wallet.Id);
        return Map(created!);
    }

    public async Task<WalletResponse> Update(long userId, long walletId, UpdateWalletRequest request)
    {
        var name = request.Name.Trim();
        if (name.Length is < 3 or > 100)
        {
            throw new ValidationException("El nombre debe tener entre 3 y 100 caracteres");
        }

        var wallet = await walletRepository.GetById(walletId)
            ?? throw new ResourceNotFoundException("La cartera no existe");

        if (wallet.UserId != userId)
        {
            throw new ForbiddenException("No tienes acceso a esta cartera");
        }

        wallet.Name = name;
        wallet.UpdatedAt = DateTime.UtcNow;
        wallet.UpdatedBy = userId;

        walletRepository.Update(wallet);
        await unitOfWork.SaveChangesAsync();

        return Map(wallet);
    }

    public async Task<WalletResponse> SetDefault(long userId, long walletId)
    {
        var wallets = await walletRepository.GetActiveByUserId(userId);
        var target = wallets.FirstOrDefault(w => w.Id == walletId)
            ?? throw new ResourceNotFoundException("La cartera no existe");

        foreach (var wallet in wallets.Where(w => w.IsDefault && w.Id != target.Id))
        {
            wallet.IsDefault = false;
            wallet.UpdatedAt = DateTime.UtcNow;
            walletRepository.Update(wallet);
        }

        target.IsDefault = true;
        target.UpdatedAt = DateTime.UtcNow;
        walletRepository.Update(target);

        await unitOfWork.SaveChangesAsync();

        return Map(target);
    }

    public async Task Delete(long userId, long walletId)
    {
        var wallets = await walletRepository.GetActiveByUserId(userId);
        var target = wallets.FirstOrDefault(w => w.Id == walletId)
            ?? throw new ResourceNotFoundException("La cartera no existe");

        if (wallets.Count <= 1)
        {
            throw new ValidationException("Debes tener al menos una cartera activa");
        }

        var deletedStatus = await catalogRepository.GetStatusByCode(StatusCodes.Deleted)
            ?? throw new ResourceNotFoundException("El estado 'eliminado' no existe");

        target.StatusId = deletedStatus.Id;
        target.UpdatedAt = DateTime.UtcNow;
        target.UpdatedBy = userId;
        walletRepository.Update(target);

        if (target.IsDefault)
        {
            var replacement = wallets.First(w => w.Id != target.Id);
            replacement.IsDefault = true;
            replacement.UpdatedAt = DateTime.UtcNow;
            walletRepository.Update(replacement);
        }

        await unitOfWork.SaveChangesAsync();
    }

    private static WalletResponse Map(Wallet wallet) => new()
    {
        Id = wallet.Id,
        Name = wallet.Name,
        CurrencyId = wallet.CurrencyId,
        CurrencyCode = wallet.Currency.Code,
        CurrencySymbol = wallet.Currency.Symbol,
        CurrentBalance = wallet.CurrentBalance,
        IsDefault = wallet.IsDefault,
    };
}
