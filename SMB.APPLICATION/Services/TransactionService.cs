using SMB.APPLICATION.DTOs.Transaction;
using SMB.APPLICATION.Exceptions;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;
using SMB.DOMAIN.Constants;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Services;

public class TransactionService(
    ITransactionRepository transactionRepository,
    IWalletRepository walletRepository,
    ICatalogRepository catalogRepository,
    IUnitOfWork unitOfWork,
    INotificationService notificationService) : ITransactionService
{
    public async Task<long> Create(CreateTransactionRequest request, long userId)
    {
        if (request.Amount <= 0)
            throw new ValidationException("El monto debe ser mayor a cero");

        var wallet = await walletRepository.GetById(request.WalletId)
            ?? throw new ResourceNotFoundException("La cartera no existe");

        if (wallet.UserId != userId)
            throw new ForbiddenException("No tienes acceso a esta cartera");

        var transactionType = await catalogRepository.GetTransactionTypeById(request.TransactionTypeId)
            ?? throw new ResourceNotFoundException("El tipo de transacción no existe");

        var transaction = new Transaction
        {
            UserId = userId,
            WalletId = request.WalletId,
            TransactionTypeId = request.TransactionTypeId,
            CategoryId = request.CategoryId,
            SubcategoryId = request.SubcategoryId,
            PaymentMethodId = request.PaymentMethodId,
            CurrencyId = request.CurrencyId,
            Amount = request.Amount,
            ExchangeRate = request.ExchangeRate,
            AmountInDefaultCurrency = request.ExchangeRate.HasValue
                ? request.Amount * request.ExchangeRate.Value
                : request.Amount,
            Description = request.Description?.Trim(),
            TransactionDate = request.TransactionDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        if (transactionType.Code == TransactionTypesCodes.Income)
        {
            wallet.CurrentBalance += request.Amount;
        }
        else
        {
            wallet.CurrentBalance -= request.Amount;
        }

        walletRepository.Update(wallet);
        await transactionRepository.Add(transaction);
        await unitOfWork.SaveChangesAsync();

        if (wallet.IsDefault)
        {
            await notificationService.CheckBalanceAlert(userId, wallet.CurrentBalance, wallet.Currency.Symbol);
        }

        return transaction.Id;
    }

    public async Task<List<TransactionResponse>> GetAll(long userId, long? walletId)
    {
        Wallet? wallet;

        if (walletId is not null)
        {
            wallet = await walletRepository.GetById(walletId.Value)
                ?? throw new ResourceNotFoundException("La cartera no existe");

            if (wallet.UserId != userId)
                throw new ForbiddenException("No tienes acceso a esta cartera");
        }
        else
        {
            wallet = await walletRepository.GetDefaultByUserId(userId)
                ?? throw new ResourceNotFoundException("No se encontró una cartera predeterminada");
        }

        return await transactionRepository.GetByWalletId(wallet.Id);
    }
}