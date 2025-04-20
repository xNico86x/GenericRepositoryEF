using System.Linq.Expressions;

namespace GenericRepositoryEF.Infrastructure.Extensions
{
    /// <summary>
    /// Builder for predicates.
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that evaluates to true.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>A predicate that evaluates to true.</returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return entity => true;
        }

        /// <summary>
        /// Creates a predicate that evaluates to false.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>A predicate that evaluates to false.</returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return entity => false;
        }

        /// <summary>
        /// Creates a predicate with the logical OR operator.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="left">The left expression.</param>
        /// <param name="right">The right expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var invokedRight = Expression.Invoke(right, left.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left.Body, invokedRight), left.Parameters);
        }

        /// <summary>
        /// Creates a predicate with the logical AND operator.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="left">The left expression.</param>
        /// <param name="right">The right expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var invokedRight = Expression.Invoke(right, left.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left.Body, invokedRight), left.Parameters);
        }

        /// <summary>
        /// Creates a predicate with the logical NOT operator.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="expression">The expression to negate.</param>
        /// <returns>The negated expression.</returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Not(expression.Body), expression.Parameters);
        }

        /// <summary>
        /// Creates a predicate that compares a property to a value.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertySelector">The property selector.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>The comparison expression.</returns>
        public static Expression<Func<T, bool>> Equal<T, TProperty>(Expression<Func<T, TProperty>> propertySelector, TProperty value)
        {
            var parameter = propertySelector.Parameters[0];
            var property = propertySelector.Body;
            var constant = Expression.Constant(value, typeof(TProperty));
            var equality = Expression.Equal(property, constant);
            return Expression.Lambda<Func<T, bool>>(equality, parameter);
        }

        /// <summary>
        /// Creates a predicate that compares a property to a value.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertySelector">The property selector.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>The comparison expression.</returns>
        public static Expression<Func<T, bool>> NotEqual<T, TProperty>(Expression<Func<T, TProperty>> propertySelector, TProperty value)
        {
            var parameter = propertySelector.Parameters[0];
            var property = propertySelector.Body;
            var constant = Expression.Constant(value, typeof(TProperty));
            var inequality = Expression.NotEqual(property, constant);
            return Expression.Lambda<Func<T, bool>>(inequality, parameter);
        }

        /// <summary>
        /// Creates a predicate that compares a property to a value.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertySelector">The property selector.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>The comparison expression.</returns>
        public static Expression<Func<T, bool>> GreaterThan<T, TProperty>(Expression<Func<T, TProperty>> propertySelector, TProperty value)
            where TProperty : IComparable<TProperty>
        {
            var parameter = propertySelector.Parameters[0];
            var property = propertySelector.Body;
            var constant = Expression.Constant(value, typeof(TProperty));
            var comparison = Expression.GreaterThan(property, constant);
            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }

        /// <summary>
        /// Creates a predicate that compares a property to a value.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertySelector">The property selector.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>The comparison expression.</returns>
        public static Expression<Func<T, bool>> GreaterThanOrEqual<T, TProperty>(Expression<Func<T, TProperty>> propertySelector, TProperty value)
            where TProperty : IComparable<TProperty>
        {
            var parameter = propertySelector.Parameters[0];
            var property = propertySelector.Body;
            var constant = Expression.Constant(value, typeof(TProperty));
            var comparison = Expression.GreaterThanOrEqual(property, constant);
            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }

        /// <summary>
        /// Creates a predicate that compares a property to a value.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertySelector">The property selector.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>The comparison expression.</returns>
        public static Expression<Func<T, bool>> LessThan<T, TProperty>(Expression<Func<T, TProperty>> propertySelector, TProperty value)
            where TProperty : IComparable<TProperty>
        {
            var parameter = propertySelector.Parameters[0];
            var property = propertySelector.Body;
            var constant = Expression.Constant(value, typeof(TProperty));
            var comparison = Expression.LessThan(property, constant);
            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }

        /// <summary>
        /// Creates a predicate that compares a property to a value.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertySelector">The property selector.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>The comparison expression.</returns>
        public static Expression<Func<T, bool>> LessThanOrEqual<T, TProperty>(Expression<Func<T, TProperty>> propertySelector, TProperty value)
            where TProperty : IComparable<TProperty>
        {
            var parameter = propertySelector.Parameters[0];
            var property = propertySelector.Body;
            var constant = Expression.Constant(value, typeof(TProperty));
            var comparison = Expression.LessThanOrEqual(property, constant);
            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }
    }
}