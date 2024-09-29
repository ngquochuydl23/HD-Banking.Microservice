namespace HD.Wallet.Transaction.Service.FilterQueries
{
    public class TransactionFilterQuery
    {
        public string? TransactionType { get; set; }

        public string? TransactionStatus { get; set; }

        public DateTime? TransactionDateMin { get; set; }

        public DateTime? TransactionDateMax { get; set; }

        public double? AmountIn {  get; set; }
    }
}
