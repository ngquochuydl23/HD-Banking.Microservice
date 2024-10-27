using HD.Wallet.Shared.SharedDtos.Transactions;

namespace HD.Wallet.Transaction.Service.Producers.TransactionProducer
{
    public interface ITransactionProducer
    {
        Task ProduceTransaction(TransactionDto transaction);
    }
}
