namespace HD.Wallet.Transaction.Service.Infrastructure.Transactions
{
    public class AccountBankValueObject
    {
        public string Bin { get; set; }

        public string OwnerName { get; set; }

        public string AccountNo { get; set; }

        public string BankName { get; set; }

        public string? ShortName { get; set; }

        public string? LogoUrl { get; set; }

        public string BankFullName { get; set; }
    }
}
