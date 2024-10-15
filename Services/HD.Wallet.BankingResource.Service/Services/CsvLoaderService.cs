using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System;
using HD.Wallet.Shared.SharedDtos.Banks;

namespace HD.Wallet.BankingResource.Service.Services
{
    public class CsvLoaderService : IServiceCsvLoader
    {
        private readonly List<BankDto> _banks;

        public CsvLoaderService()
        {
            _banks = LoadCsv("Data/banking_resource_production.csv");
        }

        private List<BankDto> LoadCsv(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            return csv.GetRecords<BankDto>().ToList();
        }

        public List<BankDto> GetAllBanks()
        {
            return _banks;
        }
    }
}
