namespace HD.Wallet.Transaction.Service.Dtos.Transfers
{
    public class RequestTransferDto
    {
        public string SourceAccountId { get;set; }

        public string DestinationAccoutId { get; set; }

        public string TransferContent { get; set; }

        public double TransferAmount { get; set; }
    }
}
