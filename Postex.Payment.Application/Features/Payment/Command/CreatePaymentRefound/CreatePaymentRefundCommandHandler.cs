

namespace Postex.Payment.Application.Features.Payment;

public partial class CreatePaymentRefundCommandHandler : IRequestHandler<CreatePaymentRefundCommand, BaseResponse<CreatePaymentRefundResponse>>
{
    private readonly IPaymentMethodResolver _paymentMethodResolver;
    private readonly ILogger<CreatePaymentRefundCommandHandler> _logger;
    private readonly IWriteRepository<PaymentRequest> _writeRepository;
    private readonly IReadRepository<PaymentRequest> _readRepository;
    public CreatePaymentRefundCommandHandler( IReadRepository<PaymentRequest> readRepository, IPaymentMethodResolver paymentMethodResolver, ILogger<CreatePaymentRefundCommandHandler> logger,
        IWriteRepository<PaymentRequest> writeRepository)
    {
        _paymentMethodResolver = paymentMethodResolver ?? throw new ArgumentNullException(nameof(paymentMethodResolver));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
        _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
    }

    public async Task<BaseResponse<CreatePaymentRefundResponse>> Handle(CreatePaymentRefundCommand request, CancellationToken cancellationToken)
    {
        PaymentRequest paymentRequest = await _readRepository.Table.Include(p=>p.PaymentRequestRefunds).FirstOrDefaultAsync(p => p.GID == request.PaymentRequestId);
        if(paymentRequest == null)
        {
            throw new EntityNotFoundException($"{nameof(paymentRequest)} is null");
        }

        _logger.LogInformation(" Payment Process Refund is Starting - PaymentRequestInfo: {@payment}", paymentRequest);
        // Save Payment Refund
        var refund= paymentRequest.AddRefund( request.UserID, request.Amount, request.Remark);

         _writeRepository.Update(paymentRequest);
        await _writeRepository.SaveChangeAsync(cancellationToken);
        _logger.LogInformation(" Payment Request Refund Registered - PaymentRequestInfo: {@payment}", paymentRequest);
       
        // Start Refund Payment
        var paymentService= _paymentMethodResolver.Resolve(paymentRequest.PaymentMethod);
        await paymentService.RefundAsync(refund, paymentRequest, cancellationToken);
      
        _logger.LogInformation(" Payment Request Refund Operation is done Successfully - PaymentRequestInfo: {@payment}", paymentRequest);
        BaseResponse<CreatePaymentRefundResponse> response = new()
        {
            IsSuccess = true,
        };
        
        return response;
    }
}
