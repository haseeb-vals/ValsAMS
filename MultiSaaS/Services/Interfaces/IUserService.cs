using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;

namespace MultiSaaS.Services.Interfaces
{
    /// <summary>
    /// Service interface for user operations.
    /// Provides business logic for managing users.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets all users for the current tenant.
        /// </summary>
        /// <returns>A collection of users.</returns>
        Task<IEnumerable<User>> GetAllAsync();
        
        /// <summary>
        /// Gets all users for a specific vehicle.
        /// </summary>
        /// <param name="vehicleId">The vehicle ID.</param>
        /// <returns>A collection of users associated with the vehicle.</returns>
        Task<IEnumerable<User>> GetByVehicleIdAsync(int vehicleId);
        
        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The user if found, otherwise null.</returns>
        Task<User> GetByIdAsync(int id);
        
        /// <summary>
        /// Gets a user by their username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The user if found, otherwise null.</returns>
        Task<User> GetByUsernameAsync(string username);
        
        /// <summary>
        /// Gets a user by their email address.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>The user if found, otherwise null.</returns>
        Task<User> GetByEmailAsync(string email);
        
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>The created user with its ID assigned.</returns>
        Task<User> CreateAsync(User user);
        
        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="user">The user with updated values.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        Task<bool> UpdateAsync(User user);
        
        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}