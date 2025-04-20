namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines a service for accessing information about the current user.
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
    }
}