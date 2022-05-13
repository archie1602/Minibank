namespace Minibank.Web.Mappers
{
    public class MinibankWebMapper : Profile
    {
        public MinibankWebMapper()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();

            CreateMap<BankAccount, BankAccountDto>().ReverseMap();

            CreateMap<TransferHistory, TransferHistoryDto>().ReverseMap();
        }
    }
}