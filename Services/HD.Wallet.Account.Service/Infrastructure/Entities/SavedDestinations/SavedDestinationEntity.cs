using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared.Seedworks;
using System.ComponentModel.DataAnnotations.Schema;

namespace HD.Wallet.Account.Service.Infrastructure.Entities.SavedDestinations
{
    public class SavedDestinationEntity: Entity<long>
    {
        public string? ReferenceUserId { get; set; }

        public string UserId {  get; set; }

        public bool IsBankLinking { get; set; }


        [Column(TypeName = "jsonb")]
        public string? AccountBankJson { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual UserEntity? ReferenceUser { get; set; }
    }
}
