using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.SharedDtos.Transactions
{
    public class TransactionAccountBankDto
    {
        public string Bin { get; set; }

        public string OwnerName { get; set; }

        public string AccountNo { get; set; }

        public string BankName { get; set; }

        public string? ShortName { get; set; }

        public string? LogoUrl { get; set; }

        public string BankFullName { get; set; }
    }
}
