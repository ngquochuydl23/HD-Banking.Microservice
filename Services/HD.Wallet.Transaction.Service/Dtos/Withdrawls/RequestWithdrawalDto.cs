namespace HD.Wallet.Transaction.Service.Dtos.Withdrawls
{
    public class RequestWithdrawalDto
    {
        public string LinkingAccountId {  get; set; }

        public double Amount { get; set; }
    }
}
