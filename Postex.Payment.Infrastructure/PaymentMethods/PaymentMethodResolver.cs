using Postex.SharedKernel.Exceptions;

namespace Postex.Payment.Infrastructure.PaymentMethods;

public class PaymentMethodResolver : IPaymentMethodResolver
{
    private readonly IEnumerable<IPaymentMethod> _paymentMethods;
    private readonly ILogger<PaymentMethodResolver> _logger;

    public PaymentMethodResolver(IEnumerable<IPaymentMethod> paymentMethods, ILogger<PaymentMethodResolver> logger)
    {
        _paymentMethods = paymentMethods ?? throw new ArgumentNullException(nameof(paymentMethods));
        _logger = logger?? throw new ArgumentNullException(nameof(paymentMethods));
    }

    public IPaymentMethod Resolve(string paymentMethodName)
    {
        IPaymentMethod? paymentMethod=null;
        if (string.IsNullOrWhiteSpace(paymentMethodName))
        {
            paymentMethod = _paymentMethods.FirstOrDefault(q => q.IsActive);
        }
        else
        {
            paymentMethod = _paymentMethods.FirstOrDefault(q => q.Name.Equals(paymentMethodName, StringComparison.InvariantCultureIgnoreCase));
        }

        if (paymentMethod == null)
        {
            _logger.LogError($"Couldn't find Payment method with type:{paymentMethodName}");
            throw new AppException($"Payment method not found - Payment Method Name:{paymentMethodName}");
        }

        return paymentMethod;
    }
}