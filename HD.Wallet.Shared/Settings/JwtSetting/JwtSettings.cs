using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.Settings.JwtSetting
{
    public class JwtSettings
    {
        public const string SectionName = "Logging";
        public string SecretKey { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}
