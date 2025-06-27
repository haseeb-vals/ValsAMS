using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;

namespace MultiSaaS.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Branch repository operations
    /// </summary>
    public interface IBranchRepository
    {
        /// <summary>
        /// Gets all branches for the current tenant
        /// </summary>
        /// <returns>A collection of branches</returns>
        Task<IEnumerable<Branch>> GetAllAsync();
        
        /// <summary>
        /// Gets all branches for a specific company
        /// </summary>
        /// <param name="companyId">The company ID</param>
        /// <returns>A collection of branches belonging to the company</returns>
        Task<IEnumerable<Branch>> GetByCompanyIdAsync(int companyId);
        
        /// <summary>
        /// Gets a branch by its ID
        /// </summary>
        /// <param name="id">The branch ID</param>
        /// <returns>The branch if found, null otherwise</returns>
        Task<Branch> GetByIdAsync(int id);
        
        /// <summary>
        /// Creates a new branch
        /// </summary>
        /// <param name="branch">The branch to create</param>
        /// <returns>The created branch with ID assigned</returns>
        Task<Branch> CreateAsync(Branch branch);
        
        /// <summary>
        /// Updates an existing branch
        /// </summary>
        /// <param name="branch">The branch to update</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> UpdateAsync(Branch branch);
        
        /// <summary>
        /// Deletes a branch (soft delete)
        /// </summary>
        /// <param name="id">The branch ID to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> DeleteAsync(int id);
    }
}