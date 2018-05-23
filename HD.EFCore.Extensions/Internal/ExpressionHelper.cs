using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HD.EFCore.Extensions.Internal
{
    internal class ExpressionHelper
    {
        public static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId<TEntity, TPrimaryKey>(TPrimaryKey id, string keyName = "Id")
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, keyName),
                Expression.Constant(id, typeof(TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        public static Expression<Func<TEntity, bool>> CreateContainsExpressionForId<TEntity, TPrimaryKey>(IEnumerable<TPrimaryKey> ids, string keyName = "Id")
        {
            var parameter = Expression.Parameter(typeof(TEntity), "q");
            var property = Expression.Property(parameter, keyName);

            var method = typeof(Enumerable).
                                GetMethods().
                                Where(x => x.Name == "Contains").
                                Single(x => x.GetParameters().Length == 2).
                                MakeGenericMethod(typeof(TPrimaryKey));

            var value = Expression.Constant(ids, typeof(IEnumerable<TPrimaryKey>));
            var containsMethod = Expression.Call(method, value, property); 

            return Expression.Lambda<Func<TEntity, bool>>(containsMethod, parameter);
        }
    }
}
