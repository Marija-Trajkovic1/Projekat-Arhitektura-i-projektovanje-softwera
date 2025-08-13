using System.Linq.Expressions;

namespace TaskIT.Repository
{
    public class RepositoryImpl<TEntity> : Repository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        protected RepositoryImpl(DbContext context)
        {
            Context = context;
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<TEntity?> GetAsync(string id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> DeleteAsync(string id)
        {
            var entity = await Context.Set<TEntity>().FindAsync(id);
            if(entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();
            return entity;
        }   

        public async Task<TEntity> UpdateAsync(string id, TEntity entity)
        {
            var existingEntity = await Context.Set<TEntity>().FindAsync(id); 
            if(existingEntity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            Context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync();
            return existingEntity;

        }

         Task<bool> Repository<TEntity>.EntityExist(string entityId)
        {
            return Context.Set<TEntity>().AnyAsync(e => EF.Property<string>(e, "Id") == entityId);
        }
    }
}
