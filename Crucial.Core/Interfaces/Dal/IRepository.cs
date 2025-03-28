namespace Crucial.Core.Interfaces.Dal;

public interface IRepository<TEntity, in TKey>
{
    Task AddAsync(TEntity entity);
    Task<TEntity?> GetAsync(TKey key);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TKey key);
}
