using AutoMapper;
using HD.Wallet.Account.Service.Infrastructure.Entities.Accounts;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Shared;
using Microsoft.AspNetCore.Mvc;
using HD.Wallet.Account.Service.Infrastructure.Entities.SavedDestinations;
using HD.Wallet.Account.Service.Dtos;
using HD.Wallet.Account.Service.ExternalServices;
using HD.Wallet.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using HD.Wallet.Account.Service.Filters;
using HD.Wallet.Shared.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace HD.Wallet.Account.Service.Controllers
{
    [Authorize]
    [Route("account-api/[controller]")]
    public class SavedController : BaseController
    {
        private readonly BankExternalService _bankExternalService;
        private readonly IEfRepository<UserEntity, string> _userRepo;
        private readonly IEfRepository<SavedDestinationEntity, long> _savedDestinationRepo;
        private readonly IEfRepository<AccountEntity, string> _accountRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SavedController(
            IEfRepository<AccountEntity, string> accountRepo,
            IEfRepository<UserEntity, string> userRepo,
            IEfRepository<SavedDestinationEntity, long> savedDestinationRepo,
            IHttpContextAccessor httpContextAccessor,
            BankExternalService bankExternalService,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(httpContextAccessor)
        {
            _accountRepo = accountRepo;
            _userRepo = userRepo;
            _bankExternalService = bankExternalService;
            _savedDestinationRepo = savedDestinationRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetSavedDestinations([FromQuery] SavedFilter query)
        {
            var savedDestinations = _savedDestinationRepo
                .GetQueryableNoTracking()
                .Include(x => x.ReferenceUser)
                .Where(x => x.UserId.Equals(LoggingUserId))
                .WhereIf(query.IsBankLinking.HasValue, x => x.IsBankLinking.Equals(query.IsBankLinking))
                .OrderByDescending(x => x.CreatedAt)
                .Skip(query.Offset)
                .Take(query.Limit)
                .ToList();

            return Ok(_mapper.Map<List<SavedDestinationDto>>(savedDestinations));
        }
  
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] RequestSaveDestination body)
        {

            if (body.IsBankAccount)
            {
                if (string.IsNullOrEmpty(body.Bin))
                {
                    throw new AppException("body.Bin must not be null or empty.");
                }

                var bankAccount = await _bankExternalService.GetCitizenAccount(body.Bin, body.AccountNo)
                    ?? throw new AppException("Bank account not found");

                if (_savedDestinationRepo
                      .GetQueryableNoTracking()
                      .Any(x => EF.Functions.JsonContains(x.AccountBankJson, JsonConvert.SerializeObject(bankAccount)) && x.IsBankLinking))
                {
                    throw new AppException("You've already saved destination.");
                }
                var savedDestination = _savedDestinationRepo.Insert(new SavedDestinationEntity()
                {
                    UserId = LoggingUserId,
                    IsBankLinking = true,
                    AccountBankJson = JsonConvert.SerializeObject(bankAccount)
                });
            
                return Ok(_mapper.Map<SavedDestinationDto>(savedDestination));
            }


            var destinationUser = _userRepo
                .GetQueryableNoTracking()
                .FirstOrDefault(x => x.PhoneNumber.Equals(body.AccountNo))
                    ?? throw new AppException("User destination not found");

            if (_savedDestinationRepo
                     .GetQueryableNoTracking()
                     .Any(x => x.ReferenceUserId.Equals(destinationUser.Id) && !x.IsBankLinking))
            {
                throw new AppException("You've already saved destination.");
            }

            var savedUser = _savedDestinationRepo.Insert(new SavedDestinationEntity()
            {
                UserId = LoggingUserId,
                IsBankLinking = false,
                AccountBankJson = null,
                ReferenceUserId = destinationUser.Id
            });

            return Ok(_mapper.Map<SavedDestinationDto>(savedUser));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var savedDestinations = _savedDestinationRepo
                .GetQueryableNoTracking()
                .FirstOrDefault(x => x.Id == id)
                    ?? throw new AppException("SaveDestination not found");


            _savedDestinationRepo.Delete(id);
            return Ok();
        }
    }
}
