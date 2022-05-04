using KUSYS.Core.Entities;
using KUSYS.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.Core.DAL.SQLExpress
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new ()
        where TContext : DbContext, new ()
    {
        public DbContext CurrentDbcontext { get; set; }

        public EfEntityRepositoryBase(TContext context)
        {
            CurrentDbcontext = context;
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return await CurrentDbcontext.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(filter);
        }

        public async Task<TEntity> GetLast(Expression<Func<TEntity, bool>> filter)
        {
            return await CurrentDbcontext.Set<TEntity>().LastOrDefaultAsync(filter);
        }

        public async Task<IList<TEntity>> GetList(Expression<Func<TEntity, bool>> filter = null, SortingExpression<TEntity> sortingExpression = null, DataPagingOptions pagingOptions = null, params string[] includes)
        {
            IQueryable<TEntity> queryable = CurrentDbcontext.Set<TEntity>();
            if (includes != null)
                if (includes.Any()) includes.ToList().ForEach(i =>
                {
                    queryable = queryable.Include(i);

                });
            if (filter != null) queryable = queryable.Where(filter);

            if (sortingExpression != null && sortingExpression.SortExpression != null)
            {
                if (sortingExpression.Desc)
                    queryable = queryable.OrderByDescending(sortingExpression.SortExpression);
                else
                    queryable = queryable.OrderBy(sortingExpression.SortExpression);
            }

            if (pagingOptions != null && (pagingOptions.PageSize.HasValue || pagingOptions.PageSize.GetValueOrDefault() > 0 && pagingOptions.PageNumber.GetValueOrDefault() > 0))
                queryable = queryable
                    .Skip(pagingOptions.PageNumber.GetValueOrDefault() * pagingOptions.PageSize.GetValueOrDefault())
                    .Take(pagingOptions.PageSize.GetValueOrDefault());

            List<TEntity> data = await queryable.AsNoTracking().ToListAsync();

            return data;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            TEntity newEntity = (await CurrentDbcontext.AddAsync(entity)).Entity;
            await CurrentDbcontext.SaveChangesAsync();

            return newEntity;
        }

        public async Task<List<TEntity>> AddRange(List<TEntity> entity)
        {
            List<TEntity> addedEntities = await Task.Run(() =>
            {
                List<TEntity> addedEntities2 = new List<TEntity>();
                entity.ForEach(x =>
                {
                    var addedEntity = CurrentDbcontext.Add(x).Entity;
                    addedEntities2.Add(addedEntity);
                });

                return addedEntities2;
            });

            await CurrentDbcontext.SaveChangesAsync();

            return addedEntities;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var newEntity = await Task.Run(() => CurrentDbcontext.Update(entity));
            await CurrentDbcontext.SaveChangesAsync();

            return newEntity.Entity;
        }

        public async Task<List<TEntity>> UpdateRange(List<TEntity> entity)
        {
            List<TEntity> updatedEntities = await Task.Run(() =>
            {
                List<TEntity> updatedEntities2 = new List<TEntity>();
                entity.ForEach(x =>
                {
                    this.CurrentDbcontext.Entry<TEntity>(x).State = EntityState.Modified;
                    var updatedEntity = CurrentDbcontext.Update(x).Entity;
                    updatedEntities2.Add(updatedEntity);
                });
                return updatedEntities2;
            });

            await CurrentDbcontext.SaveChangesAsync();

            return updatedEntities;
        }

        public async Task Delete(TEntity entity)
        {
            CurrentDbcontext.Entry(entity).State = EntityState.Detached;
            await Task.Run(() => CurrentDbcontext.Remove(entity));
        }

        async Task<TEntity> IEntityRepository<TEntity>.DeleteById(int entityId)
        {
            var entity = await Task.Run(() =>
            {
                var e = (Expression<Func<TEntity, bool>>)System.Linq.Dynamic.Core.DynamicExpressionParser
                .ParseLambda<TEntity, bool>(parsingConfig: null, createParameterCtor: false, expression: "x => x." + typeof(TEntity).Name + "Id = " + entityId, values: null);
                var entity2 = CurrentDbcontext.Set<TEntity>().SingleOrDefault(e);
                CurrentDbcontext.Remove(entity2);
                return entity2;
            });

            return entity;
        }

        public async Task DeleteRange(List<TEntity> entity)
        {
            await Task.Run(() =>
            {
                entity.ForEach(x =>
                {
                    CurrentDbcontext.Entry(x).State = EntityState.Detached;
                });
                CurrentDbcontext.RemoveRange(entity);
            });
        }

        public async Task<int> GetTotalRecordCount(Expression<Func<TEntity, bool>> filter = null, params string[] includes)
        {
            IQueryable<TEntity> queryable = CurrentDbcontext.Set<TEntity>();

            if (includes != null) if (includes.Any()) includes.ToList().ForEach(i => { queryable = queryable.Include(i); });

            if (filter != null) queryable = queryable.Where(filter);
            return await queryable.AsNoTracking().CountAsync();
        }

        private static Func<T, T> Compose<T>(Func<T, T> innerFunc, Func<T, T> outerFunc)
        {
            return arg => outerFunc(innerFunc(arg));
        }

        public async void Dispose()
        {
            await CurrentDbcontext.DisposeAsync();
        }
    }
}
