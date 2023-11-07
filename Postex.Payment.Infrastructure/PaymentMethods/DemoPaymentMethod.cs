

//namespace Postex.Payment.Infrastructure.PaymentMethods;

//public class DemoPaymentMethod : IPaymentMethod
//{
//    private readonly ILogger<DemoPaymentMethod> _logger;
//    private readonly IWriteRepository<PaymentRequest> _writeRepository;
//    private readonly IReadRepository<PaymentRequest> _readRepository;
//    public DemoPaymentMethod(ILogger<DemoPaymentMethod> logger, IWriteRepository<PaymentRequest> writeRepository, IReadRepository<PaymentRequest> readRepository)
//    {
//        _logger = logger ??throw new ArgumentNullException(nameof(logger)); ;
//        _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository)); ;
//        _readRepository = readRepository ??throw new ArgumentNullException(nameof(readRepository)); ;
//    }

//    public string Name => "demo";

//    public bool IsActive => false;

//    public Task HandleWebhookAsync(CancellationToken cancellationToken)
//    {
//        throw new NotImplementedException();
//    }

//    public Task HandleWebhookAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken)
//    {
//        throw new NotImplementedException();
//    }

   

//    public Task PayAsync(PaymentRequest paymentRequest, CancellationToken cancellationToke)
//    {

//        paymentRequest.SetAsProcessing();
//        paymentRequest.SetPaymentMethod(Name);
//        _writeRepository.Update(paymentRequest);
//        _writeRepository.SaveChangeAsync(cancellationToke);
//        return Task.CompletedTask;
//    }

//    public Task RefundAsync(PaymentRequestRefund refund, PaymentRequest payment, CancellationToken cancellationToken)
//    {
//        throw new NotImplementedException();
//    }

//    Task<string> IPaymentMethod.HandleWebhookAsync(PaymentRequest payment, CancellationToken cancellationToken)
//    {
//        throw new NotImplementedException();
//    }
//}