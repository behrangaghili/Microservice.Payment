using System.ComponentModel.DataAnnotations;

namespace Postex.SharedKernel.Domain
{
    public abstract class BaseEntity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
        public byte[] RowVersion { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime? RemovedOn { get; set; }
    }
}
