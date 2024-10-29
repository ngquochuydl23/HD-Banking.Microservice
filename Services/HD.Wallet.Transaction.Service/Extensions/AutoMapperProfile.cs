using AutoMapper;
using HD.Wallet.Shared.SharedDtos.Transactions;
using HD.Wallet.Transaction.Service.Infrastructure.Transactions;

namespace HD.Wallet.Transaction.Service.Extensions
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
            CreateMap<AccountBankValueObject, TransactionAccountBankDto>();
            CreateMap<TransactionEntity, TransactionDto>()
                 .ForMember(d => d.TransactionStatus, op => op.MapFrom(o => o.TransactionStatus.ToString()))
                 .ForMember(d => d.TransactionType, op => op.MapFrom(o => o.TransactionType.ToString()));
        }
	}
}
