

namespace Postex.Parcel.Application.Mappings
{
    public class PaymentMapping : Profile
    {
        public PaymentMapping()
        {
            CreateMap<CreatePaymentCommand, PaymentRequest>()
                 .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UserId))
                 .ForMember(dest => dest.PayerId, opt => opt.MapFrom(src =>src.PayerId));
        }
    }
}
