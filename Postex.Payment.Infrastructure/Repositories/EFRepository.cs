namespace Postex.Payment.Infrastructure.Repositories;

public class EFRepository<TEntity> : IWriteRepository<TEntity>, IReadRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDBContext dbContext;

    public EFRepository(ApplicationDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public virtual IQueryable<TEntity> Table => dbContext.Set<TEntity>();
    public virtual IQueryable<TEntity> TableNoTracking => dbContext.Set<TEntity>().AsNoTracking();

    public TEntity Add(TEntity entity)
    {
        return dbContext.Add(entity).Entity;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(entity, cancellationToken);

        return entity;
    }
    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        var dbSet = dbContext.Set<TEntity>();
        await dbSet.AddRangeAsync(entities);
        return;
    }

    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        var dbSet = dbContext.Set<TEntity>();
        dbSet.RemoveRange(entities);
        return;
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        var dbSet = dbContext.Set<TEntity>();
        dbSet.UpdateRange(entities);
        return;
    }

    public async Task CommitTransactionAsync(Action action, CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.Database.BeginTransactionAsync(cancellationToken);
            action.Invoke();
            await SaveChangeAsync(cancellationToken);
            await dbContext.Database.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await dbContext.Database.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public TEntity Delete(TEntity entity)
    {
        return dbContext.Remove(entity).Entity;
    }

    public Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(dbContext.Remove(entity).Entity);
    }

    public Task SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public TEntity Update(TEntity entity)
    {
        return dbContext.Update(entity).Entity;
    }

    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(dbContext.Update(entity).Entity);
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken)
    {
        return dbContext.Set<TEntity>().AnyAsync(cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return dbContext.Set<TEntity>().CountAsync(cancellationToken);
    }

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return dbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public Task<TEntity> GetAsync(CancellationToken cancellationToken)
    {
        return dbContext.Set<TEntity>().FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.Set<TEntity>().FindAsync(id);
    }

   
    Task<PagedList<TEntity>> IReadRepository<TEntity>.GetPageAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> AddAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public int ExecuteRawSql(string sql)
    {
        return dbContext.Database.ExecuteSqlRaw(sql);
    }

    public int ExecuteRawSql(string sql, params object[] parameters)
    {
        return dbContext.Database.ExecuteSqlRaw(sql, parameters);
    }
}
