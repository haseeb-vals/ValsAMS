using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;

namespace MultiSaaS.Repositories.Interfaces
{
    /// <summary>
    /// Interface for User repository operations
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets all users for the current tenant
        /// </summary>
        /// <returns>A collection of users</returns>
        Task<IEnumerable<User>> GetAllAsync();
        
        /// <summary>
        /// Gets all users for a specific vehicle
        /// </summary>
        /// <param name="vehicleId">The vehicle ID</param>
        /// <returns>A collection of users belonging to the vehicle</returns>
        Task<IEnumerable<User>> GetByVehicleIdAsync(int vehicleId);
        
        /// <summary>
        /// Gets a user by its ID
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <returns>The user if found, null otherwise</returns>
        Task<User> GetByIdAsync(int id);
        
        /// <summary>
        /// Gets a user by username
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>The user if found, null otherwise</returns>
        Task<User> GetByUsernameAsync(string username);
        
        /// <summary>
        /// Gets a user by email
        /// </summary>
        /// <param name="email">The email</param>
        /// <returns>The user if found, null otherwise</returns>
        Task<User> GetByEmailAsync(string email);
        
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">The user to create</param>
        /// <returns>The created user with ID assigned</returns>
        Task<User> CreateAsync(User user);
        
        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> UpdateAsync(User user);
        
        /// <summary>
        /// Deletes a user (soft delete)
        /// </summary>
        /// <param name="id">The user ID to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> DeleteAsync(int id);
    }
}