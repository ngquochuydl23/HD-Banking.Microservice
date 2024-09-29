using HD.Wallet.Shared.Seedworks;
using System.Runtime.Serialization;

namespace HD.Wallet.Transaction.Service.Infrastructure.Transactions
{
    [DataContract]
    public enum TransactionTypeEnum
    {
        [EnumMember(Value = "Withdrawal")]
        Withdrawal,

        [EnumMember(Value = "Transfer")]
        Transfer,

        [EnumMember(Value = "Payment")]
        Payment,

        [EnumMember(Value = "Refund")]
        Refund
    }
}