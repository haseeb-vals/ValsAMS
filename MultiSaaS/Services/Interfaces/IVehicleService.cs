using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSaaS.Models;

namespace MultiSaaS.Services.Interfaces
{
    /// <summary>
    /// Service interface for vehicle operations.
    /// Provides business logic for managing vehicles.
    /// </summary>
    public interface IVehicleService
    {
        /// <summary>
        /// Gets all vehicles for the current tenant.
        /// </summary>
        /// <returns>A collection of vehicles.</returns>
        Task<IEnumerable<Vehicle>> GetAllAsync();
        
        /// <summary>
        /// Gets all vehicles for a specific branch.
        /// </summary>
        /// <param name="branchId">The branch ID.</param>
        /// <returns>A collection of vehicles belonging to the branch.</returns>
        Task<IEnumerable<Vehicle>> GetByBranchIdAsync(int branchId);
        
        /// <summary>
        /// Gets a vehicle by its ID.
        /// </summary>
        /// <param name="id">The vehicle ID.</param>
        /// <returns>The vehicle if found, otherwise null.</returns>
        Task<Vehicle> GetByIdAsync(int id);
        
        /// <summary>
        /// Creates a new vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to create.</param>
        /// <returns>The created vehicle with its ID assigned.</returns>
        Task<Vehicle> CreateAsync(Vehicle vehicle);
        
        /// <summary>
        /// Updates an existing vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle with updated values.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        Task<bool> UpdateAsync(Vehicle vehicle);
        
        /// <summary>
        /// Deletes a vehicle by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}