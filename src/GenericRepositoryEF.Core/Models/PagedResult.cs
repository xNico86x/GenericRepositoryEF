namespace GenericRepositoryEF.Core.Models
{
    /// <summary>
    /// Represents a paged result.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IReadOnlyList<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

        /// <summary>
        /// Gets a value indicating whether this page has a previous page.
        /// </summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Gets a value indicating whether this page has a next page.
        /// </summary>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>
        /// Creates a new instance of <see cref="PagedResult{T}"/>.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="page">The current page.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A paged result.</returns>
        public static PagedResult<T> Create(IReadOnlyList<T> items, int totalCount, int page, int pageSize)
        {
            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// Creates an empty paged result.
        /// </summary>
        /// <param name="page">The current page.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>An empty paged result.</returns>
        public static PagedResult<T> Empty(int page, int pageSize)
        {
            return new PagedResult<T>
            {
                Items = new List<T>(),
                TotalCount = 0,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}