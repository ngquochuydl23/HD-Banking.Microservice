using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.BaseDtos
{
    public class Dto<TId>
    {
        public TId Id { get; set; }
    }
}
