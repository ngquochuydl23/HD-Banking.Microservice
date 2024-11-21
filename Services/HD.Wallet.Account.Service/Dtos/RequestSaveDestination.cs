using HD.Wallet.Account.Service.Dtos.Users;

namespace HD.Wallet.Account.Service.Dtos
{
    public class RequestSaveDestination
    {

        public bool IsBankAccount {  get; set; }

        public string AccountNo {  get; set; }

        public string? Bin {  get; set; }
    }
}
