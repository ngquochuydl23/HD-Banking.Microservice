using HD.Wallet.Shared.SharedDtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.SharedDtos.Accounts
{
    public class AccountDto
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public bool IsBankLinking { get; set; }

        public double WalletBalance { get; set; }

        public string? LinkedAccountId { get; set; }

        public int TransactionLimit { get; set; }

        public int AccountType { get; set; }

        public AccountBankDto AccountBank { get; set; }

        public bool IsBlocked { get; set; } = false;

        public bool IsUnlinked { get; set; } = false;

        public virtual PublicUserDto? User { get; set; }
    }
}
