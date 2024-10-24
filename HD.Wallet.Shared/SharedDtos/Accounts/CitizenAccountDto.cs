using HD.Wallet.Shared.SharedDtos.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.SharedDtos.Accounts
{
    public class CitizenAccountDto
    {
        public string AccountNo { get; set; } = null!;

        public string OwnerName { get; set; } = null!;

        public string IdCardNo { get; set; } = null!;

        public string BankName { get; set; } = null!;

        public string Bin { get; set; } = null!;

        public decimal Balance { get; set; }

        public DateOnly OpenedAt { get; set; }

        public string Status { get; set; } = null!;

        public virtual BankDto Bank { get; set; } = null!;
    }
}
