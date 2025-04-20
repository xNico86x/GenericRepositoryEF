namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for obtaining information about the current user.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the current user identifier.
        /// </summary>
        string? UserId { get; }

        /// <summary>
        /// Gets a value indicating whether the user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Gets the current user name.
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        IEnumerable<string> Roles { get; }
    }
}