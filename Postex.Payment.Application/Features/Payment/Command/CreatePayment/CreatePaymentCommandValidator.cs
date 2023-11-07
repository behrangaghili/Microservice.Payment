
namespace Postex.Payment.Application.Features.Payment;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(p => p.Amount)
                .Must(p=>p>0).WithMessage("مقدار ( مبلغ) باید بیشتر از صفر باشد");

        RuleFor(p => p.AppName)
                .NotEmpty().WithMessage(" نام سرویس الزامی است");

        RuleFor(p => p.ReturnUrl)
                .NotEmpty().WithMessage(" آدرس بازگشت الزامی است");

        RuleFor(p => p.CancelUrl)
                .NotEmpty().WithMessage(" آدرس کنسل الزامی است");
    }
}
