using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;
using MultiSaaS.Repositories.Interfaces;
using MultiSaaS.Services.Interfaces;
using MultiSaaS.Tenant;

namespace MultiSaaS.Services
{
    /// <summary>
    /// Service implementation for sub-user operations.
    /// </summary>
    public class SubUserService : ISubUserService
    {
        private readonly ISubUserRepository _subUserRepository;
        private readonly ITenantProvider _tenantProvider;
        
        /// <summary>
        /// Initializes a new instance of the SubUserService class.
        /// </summary>
        /// <param name="subUserRepository">The sub-user repository.</param>
        /// <param name="tenantProvider">The tenant provider.</param>
        public SubUserService(ISubUserRepository subUserRepository, ITenantProvider tenantProvider)
        {
            _subUserRepository = subUserRepository;
            _tenantProvider = tenantProvider;
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<SubUser>> GetAllAsync()
        {
            return await _subUserRepository.GetAllAsync();
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<SubUser>> GetByUserIdAsync(int userId)
        {
            return await _subUserRepository.GetByUserIdAsync(userId);
        }
        
        /// <inheritdoc/>
        public async Task<SubUser> GetByIdAsync(int id)
        {
            return await _subUserRepository.GetByIdAsync(id);
        }
        
        /// <inheritdoc/>
        public async Task<SubUser> GetByUsernameAsync(string username)
        {
            return await _subUserRepository.GetByUsernameAsync(username);
        }
        
        /// <inheritdoc/>
        public async Task<SubUser> GetByEmailAsync(string email)
        {
            return await _subUserRepository.GetByEmailAsync(email);
        }
        
        /// <inheritdoc/>
        public async Task<SubUser> CreateAsync(SubUser subUser)
        {
            // Set the tenant ID from the current context
            subUser.TenantId = _tenantProvider.GetTenantId();
            
            return await _subUserRepository.CreateAsync(subUser);
        }
        
        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(SubUser subUser)
        {
            // Ensure the tenant ID is set correctly
            subUser.TenantId = _tenantProvider.GetTenantId();
            
            return await _subUserRepository.UpdateAsync(subUser);
        }
        
        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            return await _subUserRepository.DeleteAsync(id);
        }
    }
}