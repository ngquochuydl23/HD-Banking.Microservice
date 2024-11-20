using AutoMapper;
using HD.Wallet.Account.Service.Dtos.Users;
using HD.Wallet.Account.Service.Infrastructure.Entities.Accounts;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared.SharedDtos.Accounts;
using HD.Wallet.Shared.SharedDtos.Users;

namespace HD.Wallet.Account.Service.Extensions
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<UserEntity, UserDto>();
            CreateMap<AccountBankValueObject, AccountBankDto>();
            CreateMap<AccountEntity, AccountDto>();

            CreateMap<UserEntity, PublicUserDto>();
        }
	}
}
