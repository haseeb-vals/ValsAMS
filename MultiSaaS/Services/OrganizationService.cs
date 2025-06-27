using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;
using MultiSaaS.Repositories.Interfaces;
using MultiSaaS.Services.Interfaces;
using MultiSaaS.Tenant;

namespace MultiSaaS.Services
{
    /// <summary>
    /// Service implementation for organization operations.
    /// </summary>
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ITenantProvider _tenantProvider;
        
        /// <summary>
        /// Initializes a new instance of the OrganizationService class.
        /// </summary>
        /// <param name="organizationRepository">The organization repository.</param>
        /// <param name="tenantProvider">The tenant provider.</param>
        public OrganizationService(IOrganizationRepository organizationRepository, ITenantProvider tenantProvider)
        {
            _organizationRepository = organizationRepository;
            _tenantProvider = tenantProvider;
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<Organization>> GetAllAsync()
        {
            return await _organizationRepository.GetAllAsync();
        }
        
        /// <inheritdoc/>
        public async Task<Organization> GetByIdAsync(int id)
        {
            return await _organizationRepository.GetByIdAsync(id);
        }
        
        /// <inheritdoc/>
        public async Task<Organization> CreateAsync(Organization organization)
        {
            // Set the tenant ID from the current context
            organization.TenantId = _tenantProvider.GetTenantId();
            
            return await _organizationRepository.CreateAsync(organization);
        }
        
        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(Organization organization)
        {
            // Ensure the tenant ID is set correctly
            organization.TenantId = _tenantProvider.GetTenantId();
            
            return await _organizationRepository.UpdateAsync(organization);
        }
        
        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            return await _organizationRepository.DeleteAsync(id);
        }
    }
}