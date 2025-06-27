using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;

namespace MultiSaaS.Services.Interfaces
{
    /// <summary>
    /// Service interface for sub-user operations.
    /// Provides business logic for managing sub-users.
    /// </summary>
    public interface ISubUserService
    {
        /// <summary>
        /// Gets all sub-users for the current tenant.
        /// </summary>
        /// <returns>A collection of sub-users.</returns>
        Task<IEnumerable<SubUser>> GetAllAsync();
        
        /// <summary>
        /// Gets all sub-users for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A collection of sub-users associated with the user.</returns>
        Task<IEnumerable<SubUser>> GetByUserIdAsync(int userId);
        
        /// <summary>
        /// Gets a sub-user by their ID.
        /// </summary>
        /// <param name="id">The sub-user ID.</param>
        /// <returns>The sub-user if found, otherwise null.</returns>
        Task<SubUser> GetByIdAsync(int id);
        
        /// <summary>
        /// Gets a sub-user by their username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The sub-user if found, otherwise null.</returns>
        Task<SubUser> GetByUsernameAsync(string username);
        
        /// <summary>
        /// Gets a sub-user by their email address.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>The sub-user if found, otherwise null.</returns>
        Task<SubUser> GetByEmailAsync(string email);
        
        /// <summary>
        /// Creates a new sub-user.
        /// </summary>
        /// <param name="subUser">The sub-user to create.</param>
        /// <returns>The created sub-user with its ID assigned.</returns>
        Task<SubUser> CreateAsync(SubUser subUser);
        
        /// <summary>
        /// Updates an existing sub-user.
        /// </summary>
        /// <param name="subUser">The sub-user with updated values.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        Task<bool> UpdateAsync(SubUser subUser);
        
        /// <summary>
        /// Deletes a sub-user by their ID.
        /// </summary>
        /// <param name="id">The ID of the sub-user to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}