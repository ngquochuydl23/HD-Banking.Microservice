using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.SharedDtos.Banks
{
    public class BankDto
    {
        public string ShortName { get; set; }

        public string AndroidAppId { get; set; }

        public string LogoApp { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Bin { get; set; }

        public int Top {  get; set; }
    }
}
