using Postex.SharedKernel.Paginations;

namespace Postex.SharedKernel.Interfaces
{
    public interface IReadRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Table { get; }
        IQueryable<TEntity> TableNoTracking { get; }
        Task<TEntity> GetAsync(CancellationToken cancellationToken);
        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> AnyAsync(CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<PagedList<TEntity>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);
    }

}
