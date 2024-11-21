using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared.SharedDtos.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace HD.Wallet.Account.Service.Dtos
{
    public class SavedDestinationDto
    {
        public string? ReferenceUserId { get; set; }

        public string UserId { get; set; }

        public bool IsBankLinking { get; set; }

        public string? AccountBankJson { get; set; }

        public virtual PublicUserDto User { get; set; }

        public virtual PublicUserDto? ReferenceUser { get; set; }
    }
}
