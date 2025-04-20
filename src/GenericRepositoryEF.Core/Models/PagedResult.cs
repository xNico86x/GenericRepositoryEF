namespace GenericRepositoryEF.Core.Models
{
    /// <summary>
    /// Represents a paged result.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        public IReadOnlyList<T> Items { get; }

        /// <summary>
        /// Gets the total count of items.
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Gets the page number.
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        public int TotalPages { get; }

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
        /// <param name="items">The items.</param>
        /// <param name="totalCount">The total count of items.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        public PagedResult(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        /// <summary>
        /// Creates an empty paged result.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The paged result.</returns>
        public static PagedResult<T> Empty(int pageNumber, int pageSize)
        {
            return new PagedResult<T>(new List<T>(), 0, pageNumber, pageSize);
        }
    }
}