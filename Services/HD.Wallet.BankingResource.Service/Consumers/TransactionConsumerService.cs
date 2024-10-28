using Confluent.Kafka;
using HD.Wallet.BankingResource.Service.Infrastructure;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Shared.SharedDtos.Transactions;
using Mailjet.Client.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading;
using System.Transactions;
using static Confluent.Kafka.ConfigPropertyNames;

namespace HD.Wallet.BankingResource.Service.Consumers
{
    public class TransactionConsumerService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<TransactionConsumerService> _logger;
        private readonly BankingResourceDbContext _dbContext;

        private Task _executingTask;
        private CancellationTokenSource _cts;

        public TransactionConsumerService(
            IConfiguration configuration,
            ILogger<TransactionConsumerService> logger,
            BankingResourceDbContext dbContext)
        {
            var config = new ConsumerConfig
            {
                GroupId = configuration["KafkaTransferConsumer:GroupId"],
                BootstrapServers = configuration["KafkaTransferConsumer:BootstrapServers"],
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
            _configuration = configuration;
            _dbContext = dbContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OrderProcessing Service Started");
            _consumer.Subscribe(_configuration["KafkaTransferConsumer:Topic"]);
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);
                var transaction = JsonSerializer.Deserialize<TransactionDto>(result.Message.Value);


                if (transaction != null)
                {

                    _logger.LogInformation($"Received transaction: {transaction.Id} - Amount: {transaction.Amount}");

                   // await UpdateAccountBalanceViaBankingTransfer(transaction);
                    _consumer.Commit(result);

                }
                else
                {
                    await Task.Delay(2000);
                }
            }
        }

        private async Task UpdateAccountBalanceViaBankingTransfer(TransactionDto transaction)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (transaction.UseSourceAsLinkingBank)
                {
                    var sourceAccountBank = await _dbContext.CitizenAccountBanks
                         .AsTracking()
                         .FirstOrDefaultAsync(x => x.AccountNo.Equals(transaction.SourceAccount.AccountNo))
                             ?? throw new KafkaAppException("Source account not found");

                    sourceAccountBank.Balance -= transaction.Amount;
                    await _dbContext.SaveChangesAsync();
                }

                var destAccountBank = await _dbContext.CitizenAccountBanks
                         .AsTracking()
                         .FirstOrDefaultAsync(x => x.AccountNo.Equals(transaction.DestAccount.AccountNo))
                             ?? throw new KafkaAppException("Destination account not found");

                destAccountBank.Balance += transaction.Amount;
                await _dbContext.SaveChangesAsync();

                transactionScope.Complete();

            }
        }


    }
}
