using AutoMapper;
using HD.Wallet.BankingResource.Service.Infrastructure.Entities;
using HD.Wallet.Shared.SharedDtos.Accounts;
using HD.Wallet.Shared.SharedDtos.Banks;

namespace HD.Wallet.BankingResource.Service.Extensions
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<Bank, BankDto>();
			CreateMap<CitizenAccountBank, CitizenAccountDto>();
		}
	}
}
