namespace HD.Wallet.Transaction.Service.Dtos.Transfers
{
    public class TransferAccountDto
    {
        public string AccountNo { get; set; }

        public bool IsBankLinking { get; set; }

        public string? BankName { get; set; }
    }
}
