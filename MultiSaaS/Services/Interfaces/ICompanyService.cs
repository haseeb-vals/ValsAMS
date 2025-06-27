using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;

namespace MultiSaaS.Services.Interfaces
{
    /// <summary>
    /// Service interface for company operations.
    /// Provides business logic for managing companies.
    /// </summary>
    public interface ICompanyService
    {
        /// <summary>
        /// Gets all companies for the current tenant.
        /// </summary>
        /// <returns>A collection of companies.</returns>
        Task<IEnumerable<Company>> GetAllAsync();
        
        /// <summary>
        /// Gets all companies for a specific organization.
        /// </summary>
        /// <param name="organizationId">The organization ID.</param>
        /// <returns>A collection of companies belonging to the organization.</returns>
        Task<IEnumerable<Company>> GetByOrganizationIdAsync(int organizationId);
        
        /// <summary>
        /// Gets a company by its ID.
        /// </summary>
        /// <param name="id">The company ID.</param>
        /// <returns>The company if found, otherwise null.</returns>
        Task<Company> GetByIdAsync(int id);
        
        /// <summary>
        /// Creates a new company.
        /// </summary>
        /// <param name="company">The company to create.</param>
        /// <returns>The created company with its ID assigned.</returns>
        Task<Company> CreateAsync(Company company);
        
        /// <summary>
        /// Updates an existing company.
        /// </summary>
        /// <param name="company">The company with updated values.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        Task<bool> UpdateAsync(Company company);
        
        /// <summary>
        /// Deletes a company by its ID.
        /// </summary>
        /// <param name="id">The ID of the company to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}