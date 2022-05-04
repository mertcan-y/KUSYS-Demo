using KUSYS.Core.Entities;
using KUSYS.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.Core.DAL
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        Microsoft.EntityFrameworkCore.DbContext CurrentDbcontext { get; set; }

        Task<T> Get(Expression<Func<T, bool>> filter);

        Task<T> GetLast(Expression<Func<T, bool>> filter);

        Task<IList<T>> GetList(Expression<Func<T, bool>> filter = null,
            SortingExpression<T> sortingExpression = null,
            DataPagingOptions pagingOptions = null,
            params string[] includes);

        Task<T> Add(T entity);
        Task<List<T>> AddRange(List<T> entity);
        Task<T> Update(T entity);
        Task<List<T>> UpdateRange(List<T> entity);
        Task Delete(T entity);
        Task<T> DeleteById(int entityId);
        Task DeleteRange(List<T> entity);
        Task<int> GetTotalRecordCount(Expression<Func<T, bool>> filter = null, params string[] includes);
    }
}
