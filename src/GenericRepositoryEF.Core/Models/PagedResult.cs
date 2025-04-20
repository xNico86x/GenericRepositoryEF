namespace GenericRepositoryEF.Core.Models
{
    /// <summary>
    /// Represents a paginated collection of items.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the size of each page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        /// <summary>
        /// Gets a value indicating whether there is a previous page.
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page.
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>
        /// Gets or sets the items on the current page.
        /// </summary>
        public List<T> Items { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
        /// </summary>
        public PagedResult() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
        /// </summary>
        /// <param name="items">The items on the current page.</param>
        /// <param name="count">The total number of items across all pages.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The size of each page.</param>
        public PagedResult(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalItems = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Items = items;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PagedResult{T}"/> class.
        /// </summary>
        /// <param name="source">The source collection of items.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <param name="totalItems">The total number of items across all pages.</param>
        /// <returns>A new instance of the <see cref="PagedResult{T}"/> class.</returns>
        public static PagedResult<T> Create(IEnumerable<T> source, int pageNumber, int pageSize, int totalItems)
        {
            return new PagedResult<T>
            {
                Items = source.ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }
    }
}
