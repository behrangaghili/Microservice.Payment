namespace Postex.Parcel.Domain.AggregatesModel.PaymentAggregate.Enums
{
    public enum CashoutState
    {
        INITIALIZED,
        CANCELLING,
        CANCELLED,
        IN_PROGRESS,
        TRANSFERRED,
        FAILED
    }
}
