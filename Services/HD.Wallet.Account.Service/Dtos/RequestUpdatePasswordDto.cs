namespace HD.Wallet.Account.Service.Dtos
{
    public class RequestUpdatePasswordDto
    {
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
