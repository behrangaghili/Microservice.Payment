
namespace Postex.SharedKernel.Domain;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }
}
