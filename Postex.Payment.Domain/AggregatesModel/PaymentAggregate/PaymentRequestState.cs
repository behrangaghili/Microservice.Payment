namespace Postex.Parcel.Domain.AggregatesModel.PaymentAggregate;

public enum PaymentRequestState
{
    Pending = 0,
    Processing = 1,
    Completed = 2,
    Failed = 3
}