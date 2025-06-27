using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;

namespace MultiSaaS.Repositories.Interfaces
{
    /// <summary>
    /// Interface for organization data access operations.
    /// </summary>
    public interface IOrganizationRepository
    {
        /// <summary>
        /// Gets all organizations for the current tenant.
        /// </summary>
        /// <returns>A list of organizations.</returns>
        Task<IEnumerable<Organization>> GetAllAsync();
        
        /// <summary>
        /// Gets an organization by its ID.
        /// </summary>
        /// <param name="id">The organization ID.</param>
        /// <returns>The organization if found, otherwise null.</returns>
        Task<Organization> GetByIdAsync(int id);
        
        /// <summary>
        /// Creates a new organization.
        /// </summary>
        /// <param name="organization">The organization to create.</param>
        /// <returns>The created organization with its ID.</returns>
        Task<Organization> CreateAsync(Organization organization);
        
        /// <summary>
        /// Updates an existing organization.
        /// </summary>
        /// <param name="organization">The organization to update.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        Task<bool> UpdateAsync(Organization organization);
        
        /// <summary>
        /// Deletes an organization by its ID.
        /// </summary>
        /// <param name="id">The organization ID.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}