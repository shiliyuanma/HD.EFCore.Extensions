using System;
using System.Collections.Generic;
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
            var parameterExp = Expression.Parameter(typeof(TEntity), "type");
            var propertyExp = Expression.Property(parameterExp, keyName);
            var method = typeof(IEnumerable<TPrimaryKey>).GetMethod("Contains", new[] { typeof(IEnumerable<TPrimaryKey>) });
            var someValue = Expression.Constant(ids, typeof(IEnumerable<TPrimaryKey>));
            var containsMethodExp = Expression.Call(propertyExp, method, someValue);

            return Expression.Lambda<Func<TEntity, bool>>(containsMethodExp, parameterExp);
        }
    }
}
