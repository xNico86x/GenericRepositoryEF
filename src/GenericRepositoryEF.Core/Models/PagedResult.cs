namespace GenericRepositoryEF.Core.Models
{
    /// <summary>
    /// Represents a paged result of entities.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Gets the page items.
        /// </summary>
        public IReadOnlyList<T> Items { get; private set; }

        /// <summary>
        /// Gets the total count of items.
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Gets the page number.
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// Gets a value indicating whether there is a previous page.
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page.
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
        /// </summary>
        /// <param name="items">The items on the current page.</param>
        /// <param name="totalCount">The total count of items.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        public PagedResult(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            TotalCount = totalCount > 0 ? totalCount : 0;
            PageNumber = pageNumber > 0 ? pageNumber : 1;
            PageSize = pageSize > 0 ? pageSize : 10;
        }

        /// <summary>
        /// Creates an empty paged result.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>An empty paged result.</returns>
        public static PagedResult<T> Empty(int pageNumber, int pageSize)
        {
            return new PagedResult<T>(Array.Empty<T>(), 0, pageNumber, pageSize);
        }
    }
}