using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TaskIT.Repository
{
    public interface Repository<TEntity> where TEntity: class
    {
        Task<bool> EntityExist(string entityId); 
        void Remove(TEntity entity);
        Task AddAsync(TEntity entity);
        Task<TEntity?> GetAsync(string id);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(string id, TEntity entity);
        Task<TEntity> DeleteAsync(string id);
         
        
    }
    
}
