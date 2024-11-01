using HD.Wallet.Shared;
using HD.Wallet.Shared.Attributes;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Transaction.Service.Dtos.Withdrawls;
using HD.Wallet.Transaction.Service.ExternalServices;
using Microsoft.AspNetCore.Mvc;


namespace HD.Wallet.Transaction.Service.Controllers
{
    [Route("transaction-api/[controller]")]
    public class WithdrawalController : BaseController
    {
        private readonly AccountExternalService _accountExternalService;
        private readonly BankExternalService _bankExternalService;


        public WithdrawalController(
            AccountExternalService accountExternalService,
            BankExternalService bankExternalService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) {

            _accountExternalService = accountExternalService;
            _bankExternalService = bankExternalService;
        }


        [HttpPost]
        //[ServiceFilter(typeof(PinRequiredAttribute))]
        public async Task<IActionResult> Withdrawal([FromBody] RequestWithdrawalDto body)
        {
            var sourceWalletAccount = await _accountExternalService.GetWalletAccountByNo(PhoneNumber)
                ?? throw new AppException("Source wallet account not found");



            var linkingAccount = await _accountExternalService.GetAccountById(body.LinkingAccountId)
                ?? throw new AppException("Linking account not found");

            if (!linkingAccount.UserId.Equals(LoggingUserId))
            {
                throw new AppException("This linking account is not of yours");
            }


            if (!linkingAccount.IsBankLinking)
            {
                throw new AppException("This account is not linking account");
            }

            var destBankAccount = await _bankExternalService.GetCitizenAccount(linkingAccount.AccountBank.Bin, linkingAccount.AccountBank.BankAccountId)
                ?? throw new AppException("Destination bank account not found");



            return Ok(new {
                sourceWalletAccount,
                destBankAccount
            });
        }
    }
}
