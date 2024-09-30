using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared.Seedworks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HD.Wallet.Account.Service.Infrastructure.Entities.Accounts
{
    public class AccountEntity: Entity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public string Id { get; set; }

        public string UserId {  get; set; }

        public bool IsBankLinking {  get; set; }

        public double WalletBalance {  get; set; }

        public string? LinkedAccountId { get; set; }

        public int TransactionLimit {  get; set; }

        public AccountTypeEnum AccountType { get; set; }

        public AccountBankValueObject AccountBank {  get; set; }

        public UserEntity User { get; set; }

        public bool IsBlocked { get; set; } = false;

        public bool IsUnlinked { get; set; } = false;

        public AccountEntity() 
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
