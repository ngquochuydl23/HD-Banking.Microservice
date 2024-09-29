namespace HD.Wallet.Transaction.Service.Dtos.Transfers
{
    public class RequestTransferDto
    {
        public string SenderAccountId { get; set; }

        public string ReceiverAccountId { get; set; }

        public string TransferContent { get; set; }

        public bool TransferToLinkedBank { get; set; }

        public double TransferAmount { get; set; }
        public TransferToLinkedBankDto LinkedBank { get; set; }
    }
}
