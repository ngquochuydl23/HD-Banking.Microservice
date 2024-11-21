using HD.Wallet.Shared.Filters;

namespace HD.Wallet.Account.Service.Filters
{
    public class SavedFilter: PaginationFilter
    {
        public bool? IsBankLinking { get; set; }
    }
}
