namespace HD.Wallet.Transaction.Service.Dtos.Transfers
{
    public class RequestInternalTransferDto
    {

        public string SourceAccountId { get; set; }

        public string DestWalletAccountNo { get; set; }

        public string TransferContent { get; set; }

        public double TransferAmount { get; set; }

        public bool UseLinkingBank { get; set; }
    }
}
