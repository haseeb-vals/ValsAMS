using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;

namespace MultiSaaS.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Company repository operations
    /// </summary>
    public interface ICompanyRepository
    {
        /// <summary>
        /// Gets all companies for the current tenant
        /// </summary>
        /// <returns>A collection of companies</returns>
        Task<IEnumerable<Company>> GetAllAsync();
        
        /// <summary>
        /// Gets all companies for a specific organization
        /// </summary>
        /// <param name="organizationId">The organization ID</param>
        /// <returns>A collection of companies belonging to the organization</returns>
        Task<IEnumerable<Company>> GetByOrganizationIdAsync(int organizationId);
        
        /// <summary>
        /// Gets a company by its ID
        /// </summary>
        /// <param name="id">The company ID</param>
        /// <returns>The company if found, null otherwise</returns>
        Task<Company> GetByIdAsync(int id);
        
        /// <summary>
        /// Creates a new company
        /// </summary>
        /// <param name="company">The company to create</param>
        /// <returns>The created company with ID assigned</returns>
        Task<Company> CreateAsync(Company company);
        
        /// <summary>
        /// Updates an existing company
        /// </summary>
        /// <param name="company">The company to update</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> UpdateAsync(Company company);
        
        /// <summary>
        /// Deletes a company (soft delete)
        /// </summary>
        /// <param name="id">The company ID to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> DeleteAsync(int id);
    }
}