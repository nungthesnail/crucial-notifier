namespace Subscription.Core.Interfaces;

public interface IRepository<TModel, in TId>
    where TModel : class
{
    Task AddAsync(TModel model);
    Task<TModel?> GetByIdAsync(TId id);
    Task<IEnumerable<TModel>> GetAllAsync();
    Task UpdateAsync(TModel model);
    Task DeleteAsync(TId id);
    Task CommitAsync();
}
