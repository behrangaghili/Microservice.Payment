
using Postex.Payment.Application.Contracts.Jibit.Models;

namespace Postex.Payment.Application.Features.Payment;

public partial class CreateCashoutCommandHandler : IRequestHandler<CreateCashoutCommand, BaseResponse<CreateCashoutResponse>>
{
    private readonly IMapper _mapper;
    private readonly IPaymentMethodResolver _paymentMethodResolver;
    private readonly ILogger<CreatePaymentCommandHandler> _logger;
    private readonly IWriteRepository<CashoutBatchRequest> _writeRepository;

    public CreateCashoutCommandHandler(IMapper mapper, IPaymentMethodResolver paymentMethodResolver
        , ILogger<CreatePaymentCommandHandler> logger,
        IWriteRepository<CashoutBatchRequest> writeRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _paymentMethodResolver = paymentMethodResolver ?? throw new ArgumentNullException(nameof(paymentMethodResolver));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
    }

    public async Task<BaseResponse<CreateCashoutResponse>> Handle(CreateCashoutCommand request, CancellationToken cancellationToken)
    {
        var batchRequest = _mapper.Map<CashoutBatchRequest>(request);
        batchRequest.Id = Guid.NewGuid();
        batchRequest.CreatedBy = request.UserId;
        batchRequest.PaymentMethod = request.PaymentMethodName;
        batchRequest.CashoutItemRequests = new List<CashoutItemRequest>();
        batchRequest.CashoutItemRequests = request.Items.Select(p => new CashoutItemRequest()
        {
            IBanNumber = p.IBanNumber,
            CustomerId = p.CustomerId,
            Amount = p.Amount,
            CreatedOn = DateTime.Now,
            CreatedBy = request.UserId,
            Description = p.Description
        }).ToList();
        batchRequest.CashoutItemRequests.ForEach(p => p.Id = Guid.NewGuid());
        await _writeRepository.AddAsync(batchRequest, cancellationToken);
        await _writeRepository.SaveChangeAsync(cancellationToken);

        _logger.LogInformation(" Cashout Process is Starting - CashoutBatchRequest: {@batchRequest}", batchRequest);

        // Save Cashout
        //await _writeRepository.AddAsync(batchRequest, cancellationToken);
        //await _writeRepository.SaveChangeAsync(cancellationToken);
        _logger.LogInformation(" Cashout Request Registered - CashoutBatchRequest: {@batchRequest}", batchRequest);

        // Start Cashout
        var paymentService = _paymentMethodResolver.Resolve(request.PaymentMethodName);
        var result = await paymentService.RequestCashout(batchRequest, cancellationToken);

        _logger.LogInformation(" Cashout Request Operation is done Successfully - CashoutBatchRequest: {@batchRequest}", batchRequest);
        if (result.IsSuccess)
            batchRequest.CashoutItemRequests.ForEach(p => p.CashoutState = Parcel.Domain.AggregatesModel.PaymentAggregate.Enums.CashoutState.IN_PROGRESS);
        else
            batchRequest.CashoutItemRequests.ForEach(p => p.CashoutState = Parcel.Domain.AggregatesModel.PaymentAggregate.Enums.CashoutState.FAILED);
        await _writeRepository.UpdateAsync(batchRequest);
        await _writeRepository.SaveChangeAsync(cancellationToken);
        BaseResponse<CreateCashoutResponse> response = new ()
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            Data = new CreateCashoutResponse
            {
                BatchId = batchRequest.Id,
                CashOutItems = batchRequest.CashoutItemRequests.Select(p => new CreateCashoutItemResponce {TransferId= p.Id,status= nameof(p.CashoutState),CreateDate= p.CreatedOn }).ToList()
            }
        };

        return response;
    }

}
