using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.Wrappers
{
    public class PagedResponse<T>
    {
		public int Offset { get; set; }

		public int Limit { get; set; }

		public long Total { get; set; }

		public PagedResponse() { }

		public PagedResponse(int offset, int limit, long total)
		{
			Offset = offset;
			Limit = limit;
			Total = total;
		}
	}
}
