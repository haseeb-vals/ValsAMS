using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;
using MultiSaaS.Repositories.Interfaces;
using MultiSaaS.Services.Interfaces;
using MultiSaaS.Tenant;

namespace MultiSaaS.Services
{
    /// <summary>
    /// Service implementation for branch operations.
    /// </summary>
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly ITenantProvider _tenantProvider;
        
        /// <summary>
        /// Initializes a new instance of the BranchService class.
        /// </summary>
        /// <param name="branchRepository">The branch repository.</param>
        /// <param name="tenantProvider">The tenant provider.</param>
        public BranchService(IBranchRepository branchRepository, ITenantProvider tenantProvider)
        {
            _branchRepository = branchRepository;
            _tenantProvider = tenantProvider;
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            return await _branchRepository.GetAllAsync();
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<Branch>> GetByCompanyIdAsync(int companyId)
        {
            return await _branchRepository.GetByCompanyIdAsync(companyId);
        }
        
        /// <inheritdoc/>
        public async Task<Branch> GetByIdAsync(int id)
        {
            return await _branchRepository.GetByIdAsync(id);
        }
        
        /// <inheritdoc/>
        public async Task<Branch> CreateAsync(Branch branch)
        {
            // Set the tenant ID from the current context
            branch.TenantId = _tenantProvider.GetTenantId();
            
            return await _branchRepository.CreateAsync(branch);
        }
        
        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(Branch branch)
        {
            // Ensure the tenant ID is set correctly
            branch.TenantId = _tenantProvider.GetTenantId();
            
            return await _branchRepository.UpdateAsync(branch);
        }
        
        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            return await _branchRepository.DeleteAsync(id);
        }
    }
}