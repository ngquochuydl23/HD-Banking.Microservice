using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared.Seedworks;
using System.Runtime.Serialization;

namespace HD.Wallet.Account.Service.Infrastructure.Entities.Contacts
{
    [DataContract]
    public enum ContactTypeEnum
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
