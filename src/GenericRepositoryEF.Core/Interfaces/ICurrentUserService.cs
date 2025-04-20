namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Service to get the current user information.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the current user identifier.
        /// </summary>
        string? UserId { get; }

        /// <summary>
        /// Gets a value indicating whether the current user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }
    }
}