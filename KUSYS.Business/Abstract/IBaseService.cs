using KUSYS.Core.Utilities;
using KUSYS.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.Business.Abstract
{
    public interface IBaseService<T>
    {
        Task<IDataResult<T>> GetById(int accountPlanCodeId);
        Task<IDataResult<List<T>>> GetList(FilterHelper<T> filter = null);

        Task<IDataResult<T>> Add(T entity);
        Task<IDataResult<List<T>>> AddRange(List<T> entity);

        Task<IResult> Delete(T entity);
        Task<IResult> DeleteById(int entityId);
        Task<IResult> DeleteRange(List<T> entity);

        Task<IDataResult<T>> Update(T entity);
        Task<IDataResult<List<T>>> UpdateRange(List<T> entity);
    }
}
