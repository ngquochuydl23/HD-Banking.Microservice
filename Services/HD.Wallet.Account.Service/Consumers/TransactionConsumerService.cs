
using Confluent.Kafka;
using HD.Wallet.Account.Service.Infrastructure.Entities.Accounts;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Shared.SharedDtos.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HD.Wallet.Account.Service.Consumers
{
    public class TransactionConsumerService : BackgroundService
    {

        private readonly IConfiguration _configuration;
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<TransactionConsumerService> _logger;
        private readonly IEfRepository<AccountEntity, string> _accountRepo;
        private readonly IUnitOfWork _unitOfWork;


        public TransactionConsumerService(
               IUnitOfWork unitOfWork,
               IConfiguration configuration,
               ILogger<TransactionConsumerService> logger,
               IEfRepository<AccountEntity, string> accountRepo)
        {
            _unitOfWork = unitOfWork;
            _accountRepo = accountRepo;
            _configuration = configuration;
            var config = new ConsumerConfig
            {
                GroupId = _configuration["KafkaTransferConsumer:GroupId"],
                BootstrapServers = _configuration["KafkaTransferConsumer:BootstrapServers"],
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = true
            };

            _logger = logger;
            _consumer = new ConsumerBuilder<string, string>(config)
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    Console.WriteLine($"Assigned partitions: [{string.Join(", ", partitions)}]");
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    Console.WriteLine($"Revoking assignment: [{string.Join(", ", partitions)}]");
                })
                .Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            _logger.LogInformation("TransferConsumer Service Started");
            _consumer.Subscribe(_configuration["KafkaTransferConsumer:Topic"]);


            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);
                    var transaction = JsonSerializer.Deserialize<TransactionDto>(result.Message.Value);

                    if (transaction != null)
                    {
                        _logger.LogInformation($"Received transaction: {transaction.Id} - Amount: {transaction.Amount}");
                        UpdateBalance(transaction);
                        _consumer.Commit(result);
                    }
                    else
                    {
                        _logger.LogWarning("Received null transaction, retrying in 2 seconds...");
                        await Task.Delay(2000, stoppingToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("TransactionConsumer Service is stopping.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing transactions.");
                    await Task.Delay(2000, stoppingToken);
                }

            }


        }
        private void UpdateBalance(TransactionDto transaction)
        {
            using (_unitOfWork.Begin())
            {
                if (!transaction.UseSourceAsLinkingBank)
                {
                    var sourceWallet = _accountRepo
                        .GetQueryable()
                        .FirstOrDefault(x => !x.IsBankLinking
                            && x.AccountBank.BankAccountId.Equals(transaction.SourceAccount.AccountNo))
                                ?? throw new KafkaAppException("Wallet source not found");

                    sourceWallet.WalletBalance -= transaction.Amount;

                    _accountRepo.SaveChanges();
                }

                if (!transaction.IsBankingTransfer)
                {
                    var destWallet = _accountRepo
                        .GetQueryable()
                        .FirstOrDefault(x => !x.IsBankLinking
                            && x.AccountBank.BankAccountId.Equals(transaction.DestAccount.AccountNo))
                                ?? throw new KafkaAppException("Wallet source not found");

                    destWallet.WalletBalance += transaction.Amount;

                    _accountRepo.SaveChanges();
                }
                _unitOfWork.Complete();
            }
        }
    }
}
