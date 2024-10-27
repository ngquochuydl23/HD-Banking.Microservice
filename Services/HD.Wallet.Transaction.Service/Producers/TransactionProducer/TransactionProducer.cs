using Confluent.Kafka;
using HD.Wallet.Shared.SharedDtos.Transactions;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace HD.Wallet.Transaction.Service.Producers.TransactionProducer
{
    public class TransactionProducer : ITransactionProducer
    {
        private readonly ProducerConfig _producerConfig;
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;
        public TransactionProducer(IConfiguration configuration)
        {
            
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration["KafkaTransferProducer:BootstrapServers"]
            };
            _topic = configuration["KafkaTransferProducer:Topic"];
            _producer = new ProducerBuilder<string, string>(_producerConfig).Build();
        }

        public async Task ProduceTransaction(TransactionDto transaction)
        {
            var message = new Message<string, string>
            {
                Key = transaction.Id,
                Value = JsonSerializer.Serialize(transaction)
            };


            await _producer.ProduceAsync(_topic, message);
            _producer.Flush(TimeSpan.FromSeconds(5));
        }
    }
}
