using KUSYS.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.Core.Utilities
{
    public static class ExtensionFunctions
    {
        static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)
            var map = first.Parameters
            .Select((f, i) => new { f, s = second.Parameters[i] })
            .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Type GetTypeNull<T>(this T obje)
        {
            return typeof(T);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> CreatePredicateContains<T>(CreatePredicateContainsFilter filter /*string searchTerm, List<string> aranacakKelimeIncludes, List<string> aranacakKelimeSutuns/*, List<string> includes*/)
        {
            string expressionStr = DynamicExpressionCreator(
                tObject: Activator.CreateInstance(typeof(T)),
                searchWord: filter.SearchTerm,
                searchWordIncludes: filter.SearchTermIncludes,
                searchWordColumns: filter.SearchTermColumns
                /*includes: includes*/);

            return (Expression<Func<T, bool>>)System.Linq.Dynamic.Core.DynamicExpressionParser
                .ParseLambda<T, bool>(parsingConfig: null, createParameterCtor: false, expression: expressionStr, values: null);
        }

        public static Expression<Func<T, bool>> CreatePredicateContainsTum<T>(T searchTerm, List<string> includes, List<string> NullFilter = null)
        {
            string kosulStr = DynamicExpressionCreatorAll(searchTerm, NullFilter: NullFilter);

            return (Expression<Func<T, bool>>)System.Linq.Dynamic.Core.DynamicExpressionParser
                .ParseLambda<T, bool>(parsingConfig: null, createParameterCtor: false, expression: kosulStr, values: null);
        }

        public static Expression<Func<T, bool>> CreatePredicateDateTimeStartEnd<T>(DateTime dateTime, string dateColumnName, bool isEnd)
        {
            var itemType = typeof(T);
            string propName = itemType.Name;

            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            var search = Expression.Constant(Activator.CreateInstance(itemType), itemType);

            string exp = @"x => x." + dateColumnName + (isEnd ? "<" : ">") + "\"" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            var e = (Expression<Func<T, bool>>)System.Linq.Dynamic.Core.DynamicExpressionParser
                .ParseLambda<T, bool>(parsingConfig: null, createParameterCtor: false, expression: exp, values: null);

            return e;
        }

        public class CreatePredicateContainsFilter
        {
            public string SearchTerm { get; set; }
            public List<string> SearchTermIncludes { get; set; }
            public List<string> SearchTermColumns { get; set; }
        }

        public static string DynamicExpressionCreator<T>(T tObject, string searchWord, List<string> searchWordIncludes = null, List<string> searchWordColumns = null, string parent = null, string path = null)
        {
            searchWordIncludes = searchWordIncludes == null ? new List<string>() : searchWordIncludes;
            List<string> expressions = new List<string>();
            var expressionParameter = path != null ? path + tObject.GetType().Name : tObject.GetType().Name;
            expressionParameter = "x" + expressionParameter;
            string firstValue = expressionParameter + " => ";

            foreach (var item in tObject.GetType().GetProperties())
            {
                string newPath = string.Join(".", path, item.Name).TrimStart('.');

                if (item.PropertyType.Name.Contains("Collection"))
                {
                    //if (!includes.Any(x=>x.Contains((string.IsNullOrEmpty(parent) ? "" : parent + ".") + item.Name)))
                    //    continue;
                    if (!searchWordIncludes.Contains(newPath))
                        continue;

                    var deger = Activator.CreateInstance(item.PropertyType.GetGenericArguments().FirstOrDefault());

                    if (string.IsNullOrEmpty(parent))
                    {
                        string expression = DynamicExpressionCreator(
                            tObject: deger,
                            searchWord: searchWord,
                            searchWordIncludes: searchWordIncludes,
                            searchWordColumns: searchWordColumns,
                            //includes: includes,
                            path: newPath);
                        if (!string.IsNullOrEmpty(expression.Trim(' ')))
                            expressions.Add("x" + tObject.GetType().Name + "." + item.Name + ".Any(" + expression + ")");
                    }
                    else
                    {
                        string expression = DynamicExpressionCreator(
                            tObject: deger,
                            searchWord: searchWord,
                            searchWordIncludes: searchWordIncludes,
                            searchWordColumns: searchWordColumns,
                            //includes: includes,
                            path: newPath);
                        if (!string.IsNullOrEmpty(expression.Trim(' ')))
                            expressions.Add(parent + "." + item.Name + ".Any(" + expression + ")");
                    }
                }
                else if (item.PropertyType.GetInterfaces().Contains(typeof(IEntity)))
                {
                    //if (!includes.Contains((string.IsNullOrEmpty(parent) ? "x" + nesne.GetType().Name : parent) + "." + item.Name))
                    //    continue;
                    if (!searchWordIncludes.Contains(newPath))
                        continue;

                    var obje = Activator.CreateInstance(item.PropertyType);

                    string ust = "x" + tObject.GetType().Name + "." + item.Name;
                    string expression = DynamicExpressionCreator(
                        tObject: obje,
                        searchWord: searchWord,
                        searchWordIncludes: searchWordIncludes,
                        searchWordColumns: searchWordColumns,
                        //includes: includes,
                        parent: ust,
                        path: newPath);
                    if (!string.IsNullOrEmpty(expression.Trim(' ')))
                        expressions.Add("(" + expression + ")");
                }
                else
                {
                    if (searchWordColumns != null && !searchWordColumns.Contains(item.Name))
                    { continue; }

                    if (item.PropertyType == typeof(string))
                    {
                        if (string.IsNullOrEmpty(parent))
                        {
                            expressions.Add(expressionParameter + "." + item.Name + CreateEqualExpression(searchWord));
                        }
                        else
                        {
                            expressions.Add(parent + "." + item.Name + CreateEqualExpression(searchWord));
                        }
                    }
                }
            }

            if (expressions.Count == 0) return "";
            if (!string.IsNullOrEmpty(parent))
            {
                firstValue = "";
            }
            string valueString = firstValue + string.Join(" || ", expressions);
            return valueString;
        }

        public static string CreateEqualExpression(object value)
        {
            if (value.GetType() == typeof(string))
            {
                return ".Contains(\"" + value + "\")";
            }
            else if (value.GetType() == typeof(DateTime))
            {
                return " == \"" + ((DateTime)value).ToString("yyyy.MM.dd HH.mm.ss") + "\"";
            }
            else if (value.GetType() == typeof(int) || value.GetType() == typeof(decimal) || value.GetType() == typeof(double) || value.GetType() == typeof(double) || value.GetType() == typeof(bool) || value.GetType() == typeof(sbyte))
            {
                return " == " + value;
            }
            else
            {
                throw new Exception("Type conversion error!");
            }
        }

        public static string DynamicExpressionCreatorAll<T>(T tObject, string parent = null, List<string> NullFilter = null)
        {
            List<string> expressions = new List<string>();

            string firstValue = "x" + tObject.GetType().Name + " => ";

            foreach (var item in tObject.GetType().GetProperties().Where(x => x.GetValue(tObject) != null || (NullFilter != null && x.GetValue(tObject) == null && NullFilter.Contains((string.IsNullOrEmpty(parent) ? x.Name : parent + "." + x.Name)))))
            {
                if (item.PropertyType.Name.Contains("Collection"))
                {
                    var valueList = item.GetValue(tObject) as IEnumerable;

                    if (valueList.ToDynamicList().Count > 0)
                    {
                        var value = valueList.ToDynamicList<IEntity>().FirstOrDefault();

                        if (string.IsNullOrEmpty(parent))
                        {
                            expressions.Add("x" + tObject.GetType().Name + "." + item.Name + ".Any(" + DynamicExpressionCreatorAll(value) + ")");
                        }
                        else
                        {
                            expressions.Add(parent + "." + item.Name + ".Any(" + DynamicExpressionCreatorAll(value) + ")");
                        }
                    }
                }
                else if (item.PropertyType.GetInterfaces().Contains(typeof(IEntity)))
                {
                    var obje = item.GetValue(tObject);
                    if (obje != null)
                    {
                        if (string.IsNullOrEmpty(parent))
                        {
                            string ust = "x" + tObject.GetType().Name + "." + item.Name;
                            expressions.Add("(" + DynamicExpressionCreatorAll(obje, ust) + ")");
                        }
                        else
                        {
                            string ust = parent + "." + item.Name;
                            expressions.Add("(" + DynamicExpressionCreatorAll(obje, ust) + ")");
                        }
                    }
                }
                else
                {
                    var value = item.GetValue(tObject);
                    if (value != null)
                    {
                        if (string.IsNullOrEmpty(parent))
                        {
                            expressions.Add("x" + tObject.GetType().Name + "." + item.Name + CreateEqualExpression(value));
                        }
                        else
                        {
                            expressions.Add(parent + "." + item.Name + CreateEqualExpression(value));
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(parent))
                        {
                            expressions.Add("x" + tObject.GetType().Name + "." + item.Name + " == null");
                        }
                        else
                        {
                            expressions.Add(parent + "." + item.Name + " == null");
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(parent))
            {
                firstValue = "";
            }

            string valueString = firstValue + string.Join(" && ", expressions);
            return valueString;
        }
    }

    class ParameterRebinder : ExpressionVisitor
    {
        readonly Dictionary<ParameterExpression, ParameterExpression> map;

        ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;

            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }
    }
}
