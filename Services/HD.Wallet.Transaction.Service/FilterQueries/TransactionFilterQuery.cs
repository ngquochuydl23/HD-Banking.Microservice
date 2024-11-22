using HD.Wallet.Shared.Filters;

namespace HD.Wallet.Transaction.Service.FilterQueries
{
    public class TransactionFilterQuery: PaginationFilter
    {
        public string? TransactionType { get; set; }

        public string? TransactionStatus { get; set; }

        public DateTime? TransactionDateMin { get; set; }

        public DateTime? TransactionDateMax { get; set; }

        public double? AmountIn {  get; set; }

        public bool? Sent { get; set; }

        public bool? Received { get; set; }
    }
}
