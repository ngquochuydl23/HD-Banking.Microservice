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

        
        public WithdrawalController(
            AccountExternalService accountExternalService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) {

            _accountExternalService = accountExternalService;
        
        }


        [HttpPost]
        //[ServiceFilter(typeof(PinRequiredAttribute))]
        public async Task<IActionResult> Withdrawal([FromBody] RequestWithdrawalDto body)
        {
            var linkingAccount = await _accountExternalService.GetAccountById(body.LinkingAccountId)
                ?? throw new AppException("Linking account not found");

            if (!linkingAccount.IsBankLinking)
            {
                throw new AppException("This account is not linking account");
            }

             


            return Ok();
        }
    }
}
