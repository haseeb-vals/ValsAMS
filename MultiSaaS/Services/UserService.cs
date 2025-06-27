using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;
using MultiSaaS.Repositories.Interfaces;
using MultiSaaS.Services.Interfaces;
using MultiSaaS.Tenant;

namespace MultiSaaS.Services
{
    /// <summary>
    /// Service implementation for user operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITenantProvider _tenantProvider;
        
        /// <summary>
        /// Initializes a new instance of the UserService class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="tenantProvider">The tenant provider.</param>
        public UserService(IUserRepository userRepository, ITenantProvider tenantProvider)
        {
            _userRepository = userRepository;
            _tenantProvider = tenantProvider;
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetByVehicleIdAsync(int vehicleId)
        {
            return await _userRepository.GetByVehicleIdAsync(vehicleId);
        }
        
        /// <inheritdoc/>
        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
        
        /// <inheritdoc/>
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }
        
        /// <inheritdoc/>
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }
        
        /// <inheritdoc/>
        public async Task<User> CreateAsync(User user)
        {
            // Set the tenant ID from the current context
            user.TenantId = _tenantProvider.GetTenantId();
            
            return await _userRepository.CreateAsync(user);
        }
        
        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(User user)
        {
            // Ensure the tenant ID is set correctly
            user.TenantId = _tenantProvider.GetTenantId();
            
            return await _userRepository.UpdateAsync(user);
        }
        
        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }
    }
}