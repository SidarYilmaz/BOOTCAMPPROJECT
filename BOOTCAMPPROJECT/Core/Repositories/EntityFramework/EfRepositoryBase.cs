using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.EntityFramework
{
    public class EfRepositoryBase<TEntity, TEntityId, TContext> : IRepository<TEntity, TEntityId>, IAsyncRepository<TEntity, TEntityId>
        where TEntity : BaseEntity<TEntityId>
        where TContext : DbContext
    {
        protected readonly TContext Context;
        public IQueryable<TEntity> Query() => Context.Set<TEntity>();

        protected EfRepositoryBase(TContext context)
        {
            Context = context;
        }

        public TEntity Add(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            Context.Add(entity);
            Context.SaveChanges();
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            entity.UpdatedDate = DateTime.Now;
            Context.Update(entity);
            Context.SaveChanges();
            return entity;
        }

        public TEntity Delete(TEntity entity)
        {
            entity.DeletedDate = DateTime.Now;
            Context.Remove(entity);
            Context.SaveChanges();
            return entity;
        }


        public TEntity Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            IQueryable<TEntity> query = Query();
            if (include != null)
            {
                query = include(query);
            }

            return query.FirstOrDefault(predicate);
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            IQueryable<TEntity> query = Query();

            if (include != null)
            {
                query = include(query);
            }
            if (predicate!= null)
            {
                query = query.Where(predicate);
            }

            return query.ToList();
        }



        // ASYNC Functions


        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.CreatedDate = DateTime.Now;
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;


        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.UpdatedDate = DateTime.Now;
            Context.Update(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.DeletedDate = DateTime.Now;
            Context.Remove(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = Query();
            if(include != null)
            {
                query = include(query);
            }
            if(predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.ToListAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = Query();
            if (include != null) {
                query = include(query);
            }

            return await Query().FirstOrDefaultAsync(predicate);
        }
    }
}
