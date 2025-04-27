using Core.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IAsyncRepository<TEntity, TEntityId> : IQuery<TEntity> where TEntity : BaseEntity<TEntityId>
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken) ;
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity> DeleteAsync(TEntity entity , CancellationToken cancellationToken);
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, CancellationToken cancellationToken = default);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,CancellationToken cancellationToken = default);
    }
}
