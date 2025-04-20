namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface to get the current user information.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the identifier of the current user.
        /// </summary>
        string? UserId { get; }

        /// <summary>
        /// Gets the name of the current user.
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// Gets a value indicating whether the current user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }
    }
}