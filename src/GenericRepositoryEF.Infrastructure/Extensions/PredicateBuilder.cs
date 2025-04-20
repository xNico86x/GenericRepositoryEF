using System.Linq.Expressions;

namespace GenericRepositoryEF.Infrastructure.Extensions
{
    /// <summary>
    /// Builder for creating predicates.
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that always returns true.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the predicate.</typeparam>
        /// <returns>A predicate that always returns true.</returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return param => true;
        }

        /// <summary>
        /// Creates a predicate that always returns false.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the predicate.</typeparam>
        /// <returns>A predicate that always returns false.</returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return param => false;
        }

        /// <summary>
        /// Combines two predicates with an OR operation.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the predicate.</typeparam>
        /// <param name="expr1">The first predicate.</param>
        /// <param name="expr2">The second predicate.</param>
        /// <returns>A predicate that combines the two input predicates with an OR operation.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Combines two predicates with an AND operation.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the predicate.</typeparam>
        /// <param name="expr1">The first predicate.</param>
        /// <param name="expr2">The second predicate.</param>
        /// <returns>A predicate that combines the two input predicates with an AND operation.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Negates a predicate.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the predicate.</typeparam>
        /// <param name="expr">The predicate to negate.</param>
        /// <returns>A predicate that negates the input predicate.</returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Not(expr.Body), expr.Parameters);
        }
    }
}