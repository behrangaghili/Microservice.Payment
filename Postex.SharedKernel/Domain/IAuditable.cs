namespace Postex.SharedKernel.Domain
{
    public interface IAuditable<TUserId>
        where TUserId : struct, IComparable, IComparable<TUserId>
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? RemovedOn { get; set; }

        public TUserId? CreatedBy { get; set; }
        public TUserId? ModifiedBy { get; set; }
    }
}
