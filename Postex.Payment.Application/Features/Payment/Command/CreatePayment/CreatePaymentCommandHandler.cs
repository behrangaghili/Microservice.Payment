
namespace Postex.Payment.Application.Features.Payment;

public partial class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, BaseResponse<CreatePaymentResponse>>
{
    private readonly IMapper _mapper;
    private readonly IPaymentMethodResolver _paymentMethodResolver;
    private readonly ILogger<CreatePaymentCommandHandler> _logger;
    private readonly IWriteRepository<PaymentRequest> _writeRepository;

    public CreatePaymentCommandHandler(IMapper mapper, IPaymentMethodResolver paymentMethodResolver, ILogger<CreatePaymentCommandHandler> logger,
        IWriteRepository<PaymentRequest> writeRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _paymentMethodResolver = paymentMethodResolver ?? throw new ArgumentNullException(nameof(paymentMethodResolver));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
    }

    public async Task<BaseResponse<CreatePaymentResponse>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = _mapper.Map<PaymentRequest>(request);
        _logger.LogInformation(" Payment Process is Starting - PaymentRequestInfo: {@payment}", payment);

        // Save Payment
        payment.GID = Guid.NewGuid();
        await _writeRepository.AddAsync(payment, cancellationToken);
        await _writeRepository.SaveChangeAsync(cancellationToken);
        _logger.LogInformation(" Payment Request Registered - PaymentRequestInfo: {@payment}", payment);

        // Start Payment
        var paymentService = _paymentMethodResolver.Resolve(request.PaymentMethod);
        await paymentService.PayAsync(payment, cancellationToken);

        _logger.LogInformation(" Payment Request Operation is done Successfully - PaymentRequestInfo: {@payment}", payment);
        BaseResponse<CreatePaymentResponse> response = new()
        {
            IsSuccess = true,
        };

        return response;
    }
}
