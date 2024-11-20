using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.SharedDtos.Transactions
{
    public class TransactionDto
    {
        public string Id { get; set; }

        public double Amount { get; set; }

        public TransactionAccountBankDto SourceAccount { get; set; }

        public TransactionAccountBankDto DestAccount { get; set; }

        public bool IsBankingTransfer { get; set; }

        public bool UseSourceAsLinkingBank { get; set; }

        public DateTime TransactionDate { get; set; }

        public string TransactionType { get; set; }

        public string TransactionStatus { get; set; }

        public string Description { get; set; }

        public string TransferContent { get; set; }

        public string SenderUserId { get; set; }

        public string? ReceiverUserId { get; set; }
    }
}
