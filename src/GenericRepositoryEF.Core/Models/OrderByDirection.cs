namespace GenericRepositoryEF.Core.Models
{
    /// <summary>
    /// Defines the direction for ordering query results.
    /// </summary>
    public enum OrderByDirection
    {
        /// <summary>
        /// Ascending order (A to Z, 0 to 9).
        /// </summary>
        Ascending,

        /// <summary>
        /// Descending order (Z to A, 9 to 0).
        /// </summary>
        Descending
    }
}
