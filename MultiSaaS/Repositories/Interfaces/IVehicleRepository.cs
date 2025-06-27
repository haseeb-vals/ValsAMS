using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;

namespace MultiSaaS.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Vehicle repository operations
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Gets all vehicles for the current tenant
        /// </summary>
        /// <returns>A collection of vehicles</returns>
        Task<IEnumerable<Vehicle>> GetAllAsync();
        
        /// <summary>
        /// Gets all vehicles for a specific branch
        /// </summary>
        /// <param name="branchId">The branch ID</param>
        /// <returns>A collection of vehicles belonging to the branch</returns>
        Task<IEnumerable<Vehicle>> GetByBranchIdAsync(int branchId);
        
        /// <summary>
        /// Gets a vehicle by its ID
        /// </summary>
        /// <param name="id">The vehicle ID</param>
        /// <returns>The vehicle if found, null otherwise</returns>
        Task<Vehicle> GetByIdAsync(int id);
        
        /// <summary>
        /// Creates a new vehicle
        /// </summary>
        /// <param name="vehicle">The vehicle to create</param>
        /// <returns>The created vehicle with ID assigned</returns>
        Task<Vehicle> CreateAsync(Vehicle vehicle);
        
        /// <summary>
        /// Updates an existing vehicle
        /// </summary>
        /// <param name="vehicle">The vehicle to update</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> UpdateAsync(Vehicle vehicle);
        
        /// <summary>
        /// Deletes a vehicle (soft delete)
        /// </summary>
        /// <param name="id">The vehicle ID to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> DeleteAsync(int id);
    }
}