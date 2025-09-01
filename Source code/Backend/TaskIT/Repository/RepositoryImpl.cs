using System.Linq.Expressions;
namespace TaskIT.Repository
{
    public class RepositoryImpl<TEntity> : Repository<TEntity> where TEntity : class
    {
        protected readonly TaskITContext context;
        protected RepositoryImpl(TaskITContext context)
        {
            this.context = context;
        }

        public Task<bool>EntityExist(string entityId)
        {
            return context.Set<TEntity>().AnyAsync(e => EF.Property<string>(e, "Id") == entityId);
        }

        public async Task<TEntity?> GetAsync(string id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<TEntity> UpdateAsync(string id, TEntity entity)
        {
            var existingEntity = await context.Set<TEntity>().FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            context.Entry(entity).Property("Id").CurrentValue = id;
            context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await context.SaveChangesAsync();
            return existingEntity;
        }

        public async Task<TEntity> DeleteAsync(string id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if(entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }   

        
    }
}
