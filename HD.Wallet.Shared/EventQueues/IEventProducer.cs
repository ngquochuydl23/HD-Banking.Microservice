using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.EventQueues
{
	public interface IEventProducer
	{
		Task<bool> WriteMessage(string message, string topic);
	}
}
