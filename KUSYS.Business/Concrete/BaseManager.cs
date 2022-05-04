using KUSYS.Business.Abstract;
using KUSYS.Core.Entities;
using KUSYS.Core.Utilities;
using KUSYS.Core.Utilities.Results;
using KUSYS.DAL.Abstract;
using KUSYS.DAL.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.Business.Concrete
{
    public class BaseManager<T> : IBaseService<T> where T : class, IEntity, new()
    {
        private IUnitOfWork<T> _unitOfWork;

        public BaseManager()
        {
            _unitOfWork = new UnitOfWork<T>();
        }

        public async Task<IDataResult<T>> Add(T entity)
        {
            try
            {
                var added = await _unitOfWork.Generic.Add(entity);
                await _unitOfWork.SaveAsync();
                
                return new SuccessDataResult<T>(added, "Entity added successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<T>("An error has occured", ex.Message);
            }
        }

        public async Task<IDataResult<List<T>>> AddRange(List<T> entity)
        {
            try
            {
                var added = await _unitOfWork.Generic.AddRange(entity);
                await _unitOfWork.SaveAsync();

                return new SuccessDataResult<List<T>>(added, "Entity added successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<T>>("An error has occured", ex.Message);
            }
        }

        public async Task<IResult> Delete(T entity)
        {
            try
            {
                await _unitOfWork.Generic.Delete(entity);
                await _unitOfWork.SaveAsync();

                return new SuccessResult("Entity deleted successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult("An error has occured", ex.Message);
            }
        }

        public async Task<IResult> DeleteById(int entityId)
        {
            try
            {
                await _unitOfWork.Generic.DeleteById(entityId);
                await _unitOfWork.SaveAsync();

                return new SuccessResult("Entity deleted successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult("An error has occured", ex.Message);
            }
        }

        public async Task<IResult> DeleteRange(List<T> entity)
        {
            try
            {
                await _unitOfWork.Generic.DeleteRange(entity);
                await _unitOfWork.SaveAsync();

                return new SuccessResult("Entity deleted successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult("An error has occured", ex.Message);
            }
        }

        public async Task<IDataResult<T>> GetById(int entityId)
        {
            try
            {
                string exp = @"x => x." + typeof(T).Name + "Id == \""+entityId+"\"";
                var e = (Expression<Func<T, bool>>)System.Linq.Dynamic.Core.DynamicExpressionParser
                    .ParseLambda<T, bool>(parsingConfig: null, createParameterCtor: false, expression: exp, values: null);
                return new SuccessDataResult<T>(await _unitOfWork.Generic.Get(e));
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<T>("An error has occured", ex.Message);
            }
        }

        public async Task<IDataResult<List<T>>> GetList(FilterHelper<T> filterHelper = null)
        {
            try
            {
                Expression<Func<T, bool>> expression = null;
                SortingExpression<T> sortingExpression = null;

                expression = xs => true;

                if (filterHelper != null)
                {
                    if (filterHelper.SearchWord != null && !string.IsNullOrEmpty(filterHelper.SearchWord))
                    {
                        expression = expression.And(ExtensionFunctions.CreatePredicateContains<T>(new ExtensionFunctions.CreatePredicateContainsFilter { SearchTerm = filterHelper.SearchWord, SearchTermColumns = filterHelper.SearchWordColumns, SearchTermIncludes = filterHelper.SearchWordIncludes }));
                    }

                    if (filterHelper.Data != null)
                    {
                        expression = expression.And(ExtensionFunctions.CreatePredicateContainsTum<T>(filterHelper.Data, filterHelper.Includes));
                    }

                    if (!string.IsNullOrEmpty(filterHelper.DateColumnName))
                    {
                        if (filterHelper.Data.GetTypeNull().GetProperty(filterHelper.DateColumnName) != null)
                        {
                            if (filterHelper.StartDate != null)
                            {
                                expression = expression.And(ExtensionFunctions.CreatePredicateDateTimeStartEnd<T>((DateTime)filterHelper.StartDate, filterHelper.DateColumnName, false));
                            }

                            if (filterHelper.EndDate != null)
                            {
                                expression = expression.And(ExtensionFunctions.CreatePredicateDateTimeStartEnd<T>((DateTime)filterHelper.EndDate, filterHelper.DateColumnName, true));
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(filterHelper.OrderBy))
                    {
                        if (filterHelper.Data.GetTypeNull().GetProperty(filterHelper.OrderBy) != null)
                        {
                            sortingExpression = SortingExpression<T>.Create(filterHelper.OrderBy, filterHelper.IsDesc);
                        }
                    }

                }

                return new SuccessDataResult<List<T>>
                    (
                    data: (await _unitOfWork.Generic.GetList(filter: expression,
                    sortingExpression: sortingExpression,
                    pagingOptions: filterHelper != null ? (filterHelper.PagingOptions != null ? filterHelper.PagingOptions : null) : null,
                    includes: filterHelper != null ? (filterHelper.Includes != null ? filterHelper.Includes.ToArray() : null) : null)).ToList(),
                    totalCount: await _unitOfWork.Generic.GetTotalRecordCount(filter: expression,
                    includes: filterHelper != null ? (filterHelper.Includes != null ? filterHelper.Includes.ToArray() : null) : null)
                    );
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<T>>("An error has occured", ex.Message);
            }
        }

        public async Task<IDataResult<T>> Update(T entity)
        {
            try
            {
                var updated = await _unitOfWork.Generic.Update(entity);
                await _unitOfWork.SaveAsync();

                return new SuccessDataResult<T>(updated, "Entity updated successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<T>("An error has occured", ex.Message);
            }
        }

        public async Task<IDataResult<List<T>>> UpdateRange(List<T> entity)
        {
            try
            {
                var updated = await _unitOfWork.Generic.UpdateRange(entity);
                await _unitOfWork.SaveAsync();

                return new SuccessDataResult<List<T>>(updated, "Entity updated successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<T>>("An error has occured", ex.Message);
            }
        }
    }
}
