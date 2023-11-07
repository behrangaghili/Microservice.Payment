namespace Postex.Payment.Application.Contracts;

public interface IPaymentMethodResolver
{
    IPaymentMethod Resolve(string paymentMethodName);
}