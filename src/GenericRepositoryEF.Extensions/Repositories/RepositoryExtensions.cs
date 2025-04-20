using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Models;
using GenericRepositoryEF.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Extensions.Repositories
{
    /// <summary>
    /// Extension methods for <see cref="IRepository{T}"/> and <see cref="IReadOnlyRepository{T}"/>.
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Lists all entities.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A read-only list of entities.</returns>
        public static async Task<IReadOnlyList<T>> ListAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return await repository.GetAllAsync(cancellationToken);
        }

        /// <summary>
        /// Lists entities that satisfy the specified specification.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="specification">The specification to filter entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A read-only list of filtered entities.</returns>
        public static async Task<IReadOnlyList<T>> ListAsync<T>(this IReadOnlyRepository<T> repository, ISpecification<T> specification, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return await repository.GetBySpecificationAsync(specification, cancellationToken);
        }

        /// <summary>
        /// Gets a paged list of entities that satisfy the specified specification.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="specification">The specification to filter entities.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A paged result of filtered entities.</returns>
        public static async Task<PagedResult<T>> ListPagedAsync<T>(this IReadOnlyRepository<T> repository, ISpecification<T> specification, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return await repository.GetPagedAsync(pageNumber, pageSize, specification, cancellationToken);
        }

        /// <summary>
        /// Updates an entity asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task<T> UpdateAsync<T>(this IRepository<T> repository, T entity, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            repository.Update(entity);
            await repository.SaveChangesAsync(cancellationToken);
            return entity;
        }

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task DeleteAsync<T>(this IRepository<T> repository, T entity, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            repository.Delete(entity);
            await repository.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Executes an update operation for the matched entities.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="specification">The specification to filter entities.</param>
        /// <param name="setProperties">The action that sets the properties to update.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of affected entities.</returns>
        public static async Task<int> BulkUpdateAsync<T>(this IRepository<T> repository, ISpecification<T> specification, Action<T> setProperties, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var entities = await repository.GetBySpecificationAsync(specification, cancellationToken);
            foreach (var entity in entities)
            {
                setProperties(entity);
            }
            repository.UpdateRange(entities);
            return await repository.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Executes a delete operation for the matched entities.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="specification">The specification to filter entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of affected entities.</returns>
        public static async Task<int> BulkDeleteAsync<T>(this IRepository<T> repository, ISpecification<T> specification, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var entities = await repository.GetBySpecificationAsync(specification, cancellationToken);
            repository.DeleteRange(entities);
            return await repository.SaveChangesAsync(cancellationToken);
        }
    }
}