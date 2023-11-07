
namespace Postex.Parcel.Domain.Exceptions;

public class CashoutDomainException : BusinessException
{
   
    public CashoutDomainException(string message)
        : base(message)
    { }
     
    public CashoutDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

