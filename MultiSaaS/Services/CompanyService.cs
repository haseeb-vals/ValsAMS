using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;
using MultiSaaS.Repositories.Interfaces;
using MultiSaaS.Services.Interfaces;
using MultiSaaS.Tenant;

namespace MultiSaaS.Services
{
    /// <summary>
    /// Service implementation for company operations.
    /// </summary>
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ITenantProvider _tenantProvider;
        
        /// <summary>
        /// Initializes a new instance of the CompanyService class.
        /// </summary>
        /// <param name="companyRepository">The company repository.</param>
        /// <param name="tenantProvider">The tenant provider.</param>
        public CompanyService(ICompanyRepository companyRepository, ITenantProvider tenantProvider)
        {
            _companyRepository = companyRepository;
            _tenantProvider = tenantProvider;
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _companyRepository.GetAllAsync();
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<Company>> GetByOrganizationIdAsync(int organizationId)
        {
            return await _companyRepository.GetByOrganizationIdAsync(organizationId);
        }
        
        /// <inheritdoc/>
        public async Task<Company> GetByIdAsync(int id)
        {
            return await _companyRepository.GetByIdAsync(id);
        }
        
        /// <inheritdoc/>
        public async Task<Company> CreateAsync(Company company)
        {
            // Set the tenant ID from the current context
            company.TenantId = _tenantProvider.GetTenantId();
            
            return await _companyRepository.CreateAsync(company);
        }
        
        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(Company company)
        {
            // Ensure the tenant ID is set correctly
            company.TenantId = _tenantProvider.GetTenantId();
            
            return await _companyRepository.UpdateAsync(company);
        }
        
        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            return await _companyRepository.DeleteAsync(id);
        }
    }
}