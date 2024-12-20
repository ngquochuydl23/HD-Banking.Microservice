﻿using AutoMapper;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Queries;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Shared.SharedDtos.Transactions;
using HD.Wallet.Transaction.Service.FilterQueries;
using HD.Wallet.Transaction.Service.Infrastructure.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace HD.Wallet.Transaction.Service.Controllers
{
    [Authorize]
    [Route("transaction-api/[controller]")]
    public class TransactionController : BaseController
    {
        private readonly IEfRepository<TransactionEntity, string> _transactionRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionController(
          IEfRepository<TransactionEntity, string> transactionRepo,
          IHttpContextAccessor httpContextAccessor,
          IUnitOfWork unitOfWork,
          IMapper mapper) : base(httpContextAccessor)
        {
            _transactionRepo = transactionRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult GetTransactions([FromQuery] TransactionFilterQuery filterQuery)
        {
            var transactions = _transactionRepo
                .GetQueryableNoTracking()
                .Where(x => x.ReceiverUserId.Equals(LoggingUserId) || x.SenderUserId.Equals(LoggingUserId))
                .WhereIf(!string.IsNullOrEmpty(filterQuery.TransactionStatus), x => x.TransactionStatus.Equals(filterQuery.TransactionStatus))
                .WhereIf(!string.IsNullOrEmpty(filterQuery.TransactionType), x => x.TransactionType.Equals(filterQuery.TransactionType))
                .WhereIf(filterQuery.Sent.HasValue && filterQuery.Sent.Value, x => x.SenderUserId.Equals(LoggingUserId))
                .WhereIf(filterQuery.Received.HasValue && filterQuery.Received.Value, x => x.ReceiverUserId.Equals(LoggingUserId))
                .OrderByDescending(x => x.TransactionDate)
                .Skip(filterQuery.Offset)
                .Take(filterQuery.Limit)
                .ToList();

            return Ok(_mapper.Map<IList<TransactionDto>>(transactions));
        }

        [HttpGet("{id}")]
        public IActionResult GetTransactionById(string id)
        {
            var transaction = _transactionRepo
                .GetQueryableNoTracking()
                .FirstOrDefault(x => x.Id.Equals(id))
                    ?? throw new AppException("Transaction not found");

            return Ok(_mapper.Map<TransactionDto>(transaction));
        }

        [HttpGet("RecentlyDestinations")]
        public IActionResult GetRecentlyTransferObject([FromQuery] RecentlyTranferFilterQuery filterQuery)
        {
            var destinations = _transactionRepo
               .GetQueryableNoTracking()
               .Where(x => x.SenderUserId.Equals(LoggingUserId))
               .Where(x => x.TransactionType.Equals(TransactionTypeEnum.Transfer))
               .Where(x => x.TransactionStatus.Equals(TransactionStatusEnum.Completed))
               .GroupBy(x => new
               {
                   x.DestAccount.Bin,
                   x.DestAccount.AccountNo,
               })
               .Select(x => x
                    .OrderByDescending(x => x.TransactionDate)
                    .FirstOrDefault())
               .AsEnumerable()
               .OrderByDescending(x => x.TransactionDate)
               .Skip(filterQuery.Offset)
               .Take(filterQuery.Limit)
               .Select(x => x.DestAccount)
               .ToList();

            return Ok(destinations);
        }
    }
}
