using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.SharedDtos.Accounts
{
    public class FindAccountDto
    {
        public string AccountNo { get; set; }

        public bool IsBankLinking { get; set; }

        public string? BankName { get; set; }

        public FindAccountDto()
        {

        }


        public FindAccountDto(string accountNo)
        {
            AccountNo = accountNo;
        }

        public FindAccountDto(string accountNo, bool isBankLinking, string? bankName)
        {
            AccountNo = accountNo;
            IsBankLinking = isBankLinking;
            BankName = bankName;
        }
    }
}
