using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.Core.Utilities
{
    public class SortingExpression<T>
    {
        public Expression<Func<T, dynamic>> SortExpression { get; set; }
        public bool Desc { get; set; }

        public static SortingExpression<T> Create(string expression, bool desc)
        {
            var itemTip = typeof(T);
            string propName = itemTip.Name;

            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            var search = Expression.Constant(Activator.CreateInstance(itemTip), itemTip);

            string exp = @"x => x." + expression;
            var e = (Expression<Func<T, dynamic>>)System.Linq.Dynamic.Core.DynamicExpressionParser
                .ParseLambda<T, dynamic>(parsingConfig: null, createParameterCtor: false, expression: exp, values: null);

            return new SortingExpression<T> { SortExpression = e, Desc = desc };
        }
    }
}
