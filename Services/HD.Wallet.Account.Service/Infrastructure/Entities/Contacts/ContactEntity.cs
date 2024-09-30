using HD.Wallet.Shared.Seedworks;

namespace HD.Wallet.Account.Service.Infrastructure.Entities.Contacts
{
    public class ContactEntity: Entity<long>
    {
        public string UserId { get; set; }

        public string ReferenceUserId { get; set; }
    }
}
