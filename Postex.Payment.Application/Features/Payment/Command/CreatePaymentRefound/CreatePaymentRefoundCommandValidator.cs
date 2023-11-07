
namespace Postex.Payment.Application.Features.Payment;

public class CreatePaymentRefundCommandValidator : AbstractValidator<CreatePaymentRefundCommand>
{
    public CreatePaymentRefundCommandValidator()
    {
        RuleFor(p => p.Amount)
                .Must(p=>p>0).WithMessage("مقدار ( مبلغ) باید بیشتر از صفر باشد");

        RuleFor(p => p.PaymentRequestId)
                .NotEmpty().WithMessage(" آی دی پرداخت الزامی است");
    }
}
