using AutoMapper;
using Postex.Payment.Api.Models;
using Postex.Payment.Application.Contracts;
using Postex.Payment.Application.Contracts.Jibit.Models;
using Postex.Payment.Application.Features.Payment;
using Postex.Payment.Infrastructure.PaymentMethods;

namespace Postex.Payment.Api.Controllers.v1;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/Payment")]
public class PaymentController : BaseApiControllerWithDefaultRoute
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IPaymentMethodResolver _paymentMethodResolver;
    public PaymentController(IMediator mediator,
        IMapper mapper
        , IPaymentMethodResolver paymentMethodResolver)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _paymentMethodResolver = paymentMethodResolver ?? throw new ArgumentNullException(nameof(paymentMethodResolver));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreatePaymentResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResult))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    public async Task<IActionResult> CreatePayment(
        [FromHeader(Name = "x-user-id")] Guid? userId,
        [FromHeader(Name = "x-correlation-id")] Guid correlationId,
        [FromBody] CreatePaymentCommand command)
    {
        if (userId == null) return BadRequest(new ApiResult(false, "user id not provided"));

        command.UserId = userId.Value;
        command.CorrelationID = correlationId;

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(new ApiResult(false, result.Message));
    }

    [HttpPost("cashout")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResult))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    public async Task<IActionResult> RequestCashout(
        [FromHeader(Name = "x-user-id")] Guid? userId,
        [FromHeader(Name = "x-correlation-id")] Guid correlationId,
        [FromBody] CashoutModel model)
    {
        if (userId == null) return BadRequest(new ApiResult(false, "user id not provided"));
        var command = new CreateCashoutCommand()
        {
            UserId = userId.Value,
            CorrelationID = correlationId,
            PaymentMethodName = model.PaymentMethodName,
            Items = model.Items.Select(p => new CreateCashoutItemCommand()
            {
                Amount = p.Amount,
                CustomerId = p.CustomerId,
                IBanNumber = p.IBanNumber,
                Description = p.Description
            }).ToList()
        };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(new ApiResult(false, result.Message));
    }

    [HttpGet("cashoutHistory")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResult))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    public async Task<IActionResult> TransferHistory(
      [FromHeader(Name = "x-user-id")] Guid? userId,
      [FromHeader(Name = "x-correlation-id")] Guid correlationId,
      [FromQuery] TransferHistoryRequest model)
    {
        if (userId == null) return BadRequest(new ApiResult(false, "user id not provided"));
        var paymentMethod = _paymentMethodResolver.Resolve(model.paymentMethod);
        if (paymentMethod == null)
            return BadRequest(new ApiResult(false, "روش پرداخت یافت نشد"));
        var result = await paymentMethod.TransactionHistory(model);
        if (result.IsSuccess)
            return Ok(result.Data);
        return BadRequest(result.Data);
    }
    [HttpGet("trackTransfer")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResult))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    public async Task<IActionResult> TrackTransfer(
      [FromHeader(Name = "x-user-id")] Guid? userId,
      [FromHeader(Name = "x-correlation-id")] Guid correlationId,
      [FromQuery] string paymentMethodName,
      [FromQuery] Guid? batchId,
      [FromQuery] Guid? transferId)
    {
        if (userId == null) return BadRequest(new ApiResult(false, "user id not provided"));
        var paymentMethod = _paymentMethodResolver.Resolve(paymentMethodName);
        if (paymentMethod == null)
            return BadRequest(new ApiResult(false, "روش پرداخت یافت نشد"));
        var result = await paymentMethod.TrackTransaction(batchId, transferId);
        if (result.IsSuccess)
            return Ok(result.Data);
        return BadRequest(result.Data);
    }
    [HttpPost, HttpGet]
    [Route("~/{paymentRequestId}/{paymentMethod}/webhook")]
    public async Task<IActionResult> HandleWebhookAsync([FromRoute] string paymentMethod, [FromRoute] Guid paymentRequestId)
    {
        CreateHookCommand command = new(paymentRequestId, paymentMethod);
        command.CorrelationId = Guid.NewGuid();

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Redirect(result.CallbackUrl);

        return BadRequest(new ApiResult(false, result.ErrorMessage));
    }

    [HttpPost]
    [Route("CreatePaymentRefund")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreatePaymentResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResult))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    public async Task<IActionResult> CreatePaymentRefund(
        [FromHeader(Name = "x-user-id")] Guid? userId,
        [FromHeader(Name = "x-correlation-id")] Guid correlationId,
        [FromBody] CreatePaymentRefundCommand command)
    {
        if (userId == null) return BadRequest(new ApiResult(false, "user id not provided"));
        command.UserID = userId.Value;
        command.CorrelationID = correlationId;

        var result = await _mediator.Send(command);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(new ApiResult(false, result.Message));
    }
}
