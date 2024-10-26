namespace HD.Wallet.Transaction.Service.Dtos.Transfers
{
    public class RequestBankTransferDto
    {
        public string SourceAccountId { get; set; }

        public string DestBin { get; set; }

        public string DestBankAccountNo { get; set; }

        public string TransferContent { get; set; }

        public double TransferAmount { get; set; }
    }
}
