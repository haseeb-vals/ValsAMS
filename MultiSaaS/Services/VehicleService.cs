using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;
using MultiSaaS.Repositories.Interfaces;
using MultiSaaS.Services.Interfaces;
using MultiSaaS.Tenant;

namespace MultiSaaS.Services
{
    /// <summary>
    /// Service implementation for vehicle operations.
    /// </summary>
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ITenantProvider _tenantProvider;
        
        /// <summary>
        /// Initializes a new instance of the VehicleService class.
        /// </summary>
        /// <param name="vehicleRepository">The vehicle repository.</param>
        /// <param name="tenantProvider">The tenant provider.</param>
        public VehicleService(IVehicleRepository vehicleRepository, ITenantProvider tenantProvider)
        {
            _vehicleRepository = vehicleRepository;
            _tenantProvider = tenantProvider;
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _vehicleRepository.GetAllAsync();
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<Vehicle>> GetByBranchIdAsync(int branchId)
        {
            return await _vehicleRepository.GetByBranchIdAsync(branchId);
        }
        
        /// <inheritdoc/>
        public async Task<Vehicle> GetByIdAsync(int id)
        {
            return await _vehicleRepository.GetByIdAsync(id);
        }
        
        /// <inheritdoc/>
        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            // Set the tenant ID from the current context
            vehicle.TenantId = _tenantProvider.GetTenantId();
            
            return await _vehicleRepository.CreateAsync(vehicle);
        }
        
        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(Vehicle vehicle)
        {
            // Ensure the tenant ID is set correctly
            vehicle.TenantId = _tenantProvider.GetTenantId();
            
            return await _vehicleRepository.UpdateAsync(vehicle);
        }
        
        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            return await _vehicleRepository.DeleteAsync(id);
        }
    }
}