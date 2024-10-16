using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.SharedDtos.Accounts
{
    public class AccountBankDto
    {
        public string Bin { get; set; }

        public string BankName { get; set; }

        public string BankAccountId { get; set; }

        public string BankOwnerName { get; set; }

        public string LogoUrl { get; set; }

        public string BankFullName { get; set; }

        public string IdCardNo { get; set; }
    }
}
