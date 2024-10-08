using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.Utils
{
    public class Base64Validator
    {
        public static bool IsBase64String(string base64String)
        {
            // Remove padding characters to avoid errors
            base64String = base64String.Trim();
            if (base64String.Length % 4 != 0)
            {
                return false; // Base64 strings must be a multiple of 4
            }

            try
            {
                // Attempt to decode the string
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (FormatException)
            {
                return false; // Invalid Base64 string
            }
        }
    }
}
