namespace HD.Wallet.BankingResource.Service.Dtos
{
    public class RequestAddCitizenAccountBank
    {
        public string AccountNo { get; set; } = null!;

        public string OwnerName { get; set; } = null!;

        public string IdCardNo { get; set; } = null!;

        public string BankName { get; set; } = null!;

        public string Bin { get; set; } = null!;

        public decimal Balance { get; set; }

        public DateTime OpenedAt { get; set; }

        public string Status { get; set; } = null!;
    }
}
