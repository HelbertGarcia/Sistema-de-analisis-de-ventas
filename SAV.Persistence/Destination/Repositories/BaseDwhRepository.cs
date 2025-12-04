using Microsoft.EntityFrameworkCore;
using SAV.Domain.Repository;
using System.Linq.Expressions;

namespace SAV.Persistence.Destination.Repositories
{
    public class BaseDwhRepository<TEntity> : IBaseDwhRepository<TEntity> where TEntity : class
    {
        protected readonly DwhDbContext _context;

        public BaseDwhRepository(DwhDbContext context)
        {
            _context = context;
        }

        public async Task<List<bool>> Exist(Expression<Func<TEntity, bool>> filter)
        {
            bool exists = await _context.Set<TEntity>().AnyAsync(filter);
            return new List<bool> { exists };
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task Remove(TEntity[] entities)
        {
            if (entities == null || !entities.Any()) return;

            _context.Set<TEntity>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAll(TEntity[] entities)
        {
            if (entities == null || !entities.Any()) return;

            const int batchSize = 5000;

            int total = entities.Length;
            int processed = 0;

            while (processed < total)
            {
                var batch = entities.Skip(processed).Take(batchSize).ToArray();

                await _context.Set<TEntity>().AddRangeAsync(batch);
                await _context.SaveChangesAsync();

                _context.ChangeTracker.Clear();

                processed += batch.Length;
            }
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(filter);
        }
    }
}