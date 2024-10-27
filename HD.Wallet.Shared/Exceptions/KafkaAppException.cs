using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.Exceptions
{
    public class KafkaAppException : Exception
    {
        public KafkaAppException(string msg) : base(msg)
        {
        }
    }
}
