namespace Minibank.Data.Mappers
{
    public class MinibankDataMapper : Profile
    {
        public MinibankDataMapper()
        {
            CreateMap<User, UserDbModel>().ReverseMap();
            CreateMap<BankAccount, BankAccountDbModel>().ReverseMap();
            CreateMap<TransferHistory, TransferHistoryDbModel>().ReverseMap();
        }
    }
}