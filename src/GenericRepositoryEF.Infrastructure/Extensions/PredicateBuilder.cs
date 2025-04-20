using System.Linq.Expressions;

namespace GenericRepositoryEF.Infrastructure.Extensions
{
    /// <summary>
    /// Provides methods for combining predicates.
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that always returns true.
        /// </summary>
        /// <typeparam name="T">The type of object to which the predicate applies.</typeparam>
        /// <returns>A predicate that always returns true.</returns>
        public static Expression<Func<T, bool>> True<T>() => _ => true;

        /// <summary>
        /// Creates a predicate that always returns false.
        /// </summary>
        /// <typeparam name="T">The type of object to which the predicate applies.</typeparam>
        /// <returns>A predicate that always returns false.</returns>
        public static Expression<Func<T, bool>> False<T>() => _ => false;

        /// <summary>
        /// Combines two predicates with a logical OR operation.
        /// </summary>
        /// <typeparam name="T">The type of object to which the predicates apply.</typeparam>
        /// <param name="expr1">The first predicate.</param>
        /// <param name="expr2">The second predicate.</param>
        /// <returns>A predicate that performs a logical OR operation.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null) return expr2;
            if (expr2 == null) return expr1;

            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Combines two predicates with a logical AND operation.
        /// </summary>
        /// <typeparam name="T">The type of object to which the predicates apply.</typeparam>
        /// <param name="expr1">The first predicate.</param>
        /// <param name="expr2">The second predicate.</param>
        /// <returns>A predicate that performs a logical AND operation.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null) return expr2;
            if (expr2 == null) return expr1;

            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Negates a predicate.
        /// </summary>
        /// <typeparam name="T">The type of object to which the predicate applies.</typeparam>
        /// <param name="expr">The predicate to negate.</param>
        /// <returns>A negated predicate.</returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
        {
            if (expr == null) return False<T>();

            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(expr.Body), expr.Parameters);
        }
    }
}
