using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class Repository<T> : ReadOnlyRepository<T>, IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="specificationEvaluator">The specification evaluator.</param>
        public Repository(DbContext context, ISpecificationEvaluator specificationEvaluator)
            : base(context, specificationEvaluator)
        {
        }

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        public virtual T Add(T entity)
        {
            return DbSet.Add(entity).Entity;
        }

        /// <summary>
        /// Adds a collection of entities.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <returns>The added entities.</returns>
        public virtual IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            var addedEntities = new List<T>();
            foreach (var entity in entities)
            {
                addedEntities.Add(Add(entity));
            }
            return addedEntities;
        }

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The added entity.</returns>
        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var entityEntry = await DbSet.AddAsync(entity, cancellationToken);
            return entityEntry.Entity;
        }

        /// <summary>
        /// Adds a collection of entities.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The added entities.</returns>
        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var addedEntities = new List<T>();
            foreach (var entity in entities)
            {
                addedEntities.Add(await AddAsync(entity, cancellationToken));
            }
            return addedEntities;
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        public virtual T Update(T entity)
        {
            AttachIfDetached(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// Updates a collection of entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <returns>The updated entities.</returns>
        public virtual IEnumerable<T> UpdateRange(IEnumerable<T> entities)
        {
            var updatedEntities = new List<T>();
            foreach (var entity in entities)
            {
                updatedEntities.Add(Update(entity));
            }
            return updatedEntities;
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(T entity)
        {
            AttachIfDetached(entity);
            DbSet.Remove(entity);
        }

        /// <summary>
        /// Deletes an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        public virtual void Delete(object id)
        {
            var entity = DbSet.Find(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        /// <summary>
        /// Deletes a collection of entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        /// <summary>
        /// Deletes entities based on a specification.
        /// </summary>
        /// <param name="specification">The specification to match entities to delete.</param>
        public virtual void Delete(ISpecification<T> specification)
        {
            var entities = GetAll(specification);
            DeleteRange(entities);
        }

        /// <summary>
        /// Saves changes to the repository.
        /// </summary>
        /// <returns>The number of entities saved.</returns>
        public virtual int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        /// <summary>
        /// Saves changes to the repository.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities saved.</returns>
        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Attaches an entity if it's not already tracked.
        /// </summary>
        /// <param name="entity">The entity to attach.</param>
        protected virtual void AttachIfDetached(T entity)
        {
            var entry = DbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                DbContext.Attach(entity);
            }
        }
    }
}