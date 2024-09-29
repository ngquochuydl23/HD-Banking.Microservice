using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.EventQueues
{
	public interface IEventConsumer
	{
		string ReadMessage();
	}
}
