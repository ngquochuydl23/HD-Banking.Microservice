using HD.Wallet.Shared.Seedworks;

namespace HD.Wallet.Account.Service.Infrastructure.Entities.Otps
{
    public class OtpEntity: Entity<long>
    {
        public string Key { get; set; }

        public string OtpCode {  get; set; }

        public string Type { get; set; }

        public DateTime ExpiredAt { get; set; }

        public bool Verified { get; set; } = false;
    }
}
