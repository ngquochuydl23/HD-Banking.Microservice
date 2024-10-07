using AutoMapper;
using HD.Wallet.Account.Service.Dtos.Users;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;

namespace HD.Wallet.Account.Service.Extensions
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<UserEntity, UserDto>();
			//CreateMap<DiscountRuleEntity, DiscountRuleDto>();
		}
	}
}
