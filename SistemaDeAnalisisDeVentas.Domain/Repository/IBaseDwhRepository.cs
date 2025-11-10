
using System.Linq.Expressions;

namespace SAV.Domain.Repository
{
    public interface IBaseDwhRepository<TEntity> where TEntity : class
    {
        Task SaveAll(TEntity[] entities);

        Task Remove(TEntity[] entities);

        Task<List<TEntity>> GetAll();

        Task<List<bool>> Exist(Expression<Func<TEntity, bool>> filter);
    }
}
