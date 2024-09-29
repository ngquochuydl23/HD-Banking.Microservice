
using System.Runtime.Serialization;

namespace HD.Wallet.Transaction.Service.Infrastructure.Transactions
{
    [DataContract]
    public enum TransactionStatusEnum
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Completed")]
        Completed,

        [EnumMember(Value = "Failed")]
        Failed,

        [EnumMember(Value = "Cancelled")]
        Cancelled,

        [EnumMember(Value = "Reversed")]
        Reversed
    }
}
