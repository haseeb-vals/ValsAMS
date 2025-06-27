using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;

namespace MultiSaaS.Repositories.Interfaces
{
    /// <summary>
    /// Interface for SubUser repository operations
    /// </summary>
    public interface ISubUserRepository
    {
        /// <summary>
        /// Gets all subusers for the current tenant
        /// </summary>
        /// <returns>A collection of subusers</returns>
        Task<IEnumerable<SubUser>> GetAllAsync();
        
        /// <summary>
        /// Gets all subusers for a specific user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>A collection of subusers belonging to the user</returns>
        Task<IEnumerable<SubUser>> GetByUserIdAsync(int userId);
        
        /// <summary>
        /// Gets a subuser by its ID
        /// </summary>
        /// <param name="id">The subuser ID</param>
        /// <returns>The subuser if found, null otherwise</returns>
        Task<SubUser> GetByIdAsync(int id);
        
        /// <summary>
        /// Gets a subuser by username
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>The subuser if found, null otherwise</returns>
        Task<SubUser> GetByUsernameAsync(string username);
        
        /// <summary>
        /// Gets a subuser by email
        /// </summary>
        /// <param name="email">The email</param>
        /// <returns>The subuser if found, null otherwise</returns>
        Task<SubUser> GetByEmailAsync(string email);
        
        /// <summary>
        /// Creates a new subuser
        /// </summary>
        /// <param name="subUser">The subuser to create</param>
        /// <returns>The created subuser with ID assigned</returns>
        Task<SubUser> CreateAsync(SubUser subUser);
        
        /// <summary>
        /// Updates an existing subuser
        /// </summary>
        /// <param name="subUser">The subuser to update</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> UpdateAsync(SubUser subUser);
        
        /// <summary>
        /// Deletes a subuser (soft delete)
        /// </summary>
        /// <param name="id">The subuser ID to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> DeleteAsync(int id);
    }
}