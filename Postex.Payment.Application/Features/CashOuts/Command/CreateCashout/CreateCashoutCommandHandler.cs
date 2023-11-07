using Postex.Parcel.Domain.AggregatesModel.CashoutAggregate;

namespace Postex.Parcel.Application.Features.CashOuts.Command.CreateCashout
{
    public class CreateCashoutCommandHandler
    {
        private readonly IMapper _mapper;
        private readonly IPaymentMethodResolver _paymentMethodResolver;
        private readonly ILogger<CreateCashoutCommandHandler> _logger;
        private readonly IWriteRepository<CashoutRequest> _writeRepository;
        public CreateCashoutCommandHandler(
            IMapper mapper,
            IPaymentMethodResolver paymentMethodResolver,
            ILogger<CreatePaymentCommandHandler> logger,
        IWriteRepository<PaymentRequest> writeRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _paymentMethodResolver = paymentMethodResolver ?? throw new ArgumentNullException(nameof(paymentMethodResolver));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
        }
    }
}
