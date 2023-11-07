using Postex.Payment.Application;

namespace Postex.Parcel.Application.Mappings
{
    public class CashoutMapping:Profile
    {
        public CashoutMapping()
        {
            CreateMap<CashoutModelItem,CreateCashoutItemCommand>();
            CreateMap<CreateCashoutItemCommand, CashoutModelItem>();

            CreateMap<CreateCashoutItemCommand, CashoutItemRequest>();
            CreateMap<CashoutItemRequest, CreateCashoutItemCommand>();

            CreateMap<CashoutModel,CreateCashoutCommand>();
            CreateMap<CreateCashoutCommand,CashoutModel>();

            CreateMap<CreateCashoutCommand,CashoutBatchRequest>();
        }
    }
}
