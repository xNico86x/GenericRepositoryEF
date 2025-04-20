namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for current user service.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the current user identifier.
        /// </summary>
        string? UserId { get; }

        /// <summary>
        /// Gets the current user name.
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// Gets a value indicating whether the current user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }
    }
}