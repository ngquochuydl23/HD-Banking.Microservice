using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.BaseDtos
{
    public class HttpResultDto<T>
    {
        public T Result { get; set; }
    }
}
