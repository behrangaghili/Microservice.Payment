
namespace Postex.Parcel.Domain.Exceptions;

public class PaymentDomainException : BusinessException
{
   
    public PaymentDomainException(string message)
        : base(message)
    { }
     
    public PaymentDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

