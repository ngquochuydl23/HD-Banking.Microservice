using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared.Seedworks;

namespace HD.Wallet.Account.Service.Infrastructure.Entities.Contacts
{
    public class ContactEntity: Entity<long>
    {
        public string OwnerId { get; set; }

        public string ReferenceUserId { get; set; }

        public UserEntity Owner { get; set; }

        public UserEntity ReferenceUser { get; set; }

        public ContactTypeEnum ContactType { get; set; }

        public string? SavedName { get; set; }
    }
}
