using HD.Wallet.Shared.SharedDtos.Banks;
using System;

namespace HD.Wallet.BankingResource.Service.Services
{
    public interface IServiceCsvLoader
    {
        List<BankDto> GetAllBanks();

        List<BankDto> GetTopBanks();
        
        BankDto? GetBankByBin(string bankId);
    }
}
