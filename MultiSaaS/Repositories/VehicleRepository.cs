using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MultiSaaS.Models;
using MultiSaaS.Repositories.Interfaces;
using MultiSaaS.Tenant;

namespace MultiSaaS.Repositories
{
    /// <summary>
    /// Repository implementation for vehicles.
    /// Uses ADO.NET to interact with the database.
    /// </summary>
    public class VehicleRepository : IVehicleRepository
    {
        private readonly string? _connectionString;
        private readonly ITenantProvider _tenantProvider;
        
        public VehicleRepository(IConfiguration configuration, ITenantProvider tenantProvider)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _tenantProvider = tenantProvider;
        }
        
        private async Task EnsureDatabaseCreatedAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    // Check if the table exists
                    using (var command = new SqlCommand(
                        "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TBL_Vehicles') " +
                        "SELECT 1 ELSE SELECT 0", connection))
                    {
                        var tableExists = (Convert.ToInt32(await command.ExecuteScalarAsync()) == 1);
                        
                        // Only try to create the table if it doesn't exist
                        if (!tableExists)
                        {
                            try
                            {
                                using (var createCommand = new SqlCommand(
                                    "CREATE TABLE TBL_Vehicles (" +
                                    "Id INT IDENTITY(1,1) PRIMARY KEY, " +
                                    "BranchId INT NOT NULL, " +
                                    "Name NVARCHAR(100) NOT NULL, " +
                                    "Description NVARCHAR(500), " +
                                    "VIN NVARCHAR(50), " +
                                    "LicensePlate NVARCHAR(20), " +
                                    "Make NVARCHAR(50), " +
                                    "Model NVARCHAR(50), " +
                                    "Year INT, " +
                                    "Status NVARCHAR(1) DEFAULT 'Y', " +
                                    "TenantId NVARCHAR(50) NOT NULL, " +
                                    "CreatedAt DATETIME DEFAULT GETDATE(), " +
                                    "CreatedBy NVARCHAR(50), " +
                                    "UpdatedAt DATETIME, " +
                                    "UpdatedBy NVARCHAR(50), " +
                                    "CONSTRAINT FK_Vehicles_Branches FOREIGN KEY (BranchId) " +
                                    "REFERENCES TBL_Branches(Id)" +
                                    ")", connection))
                                {
                                    await createCommand.ExecuteNonQueryAsync();
                                }
                            }
                            catch (SqlException ex)
                            {
                                // Log the error but continue - the table might already exist or user might not have permission
                                System.Diagnostics.Debug.WriteLine($"Error creating table: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't throw - we'll try to continue with the operation
                System.Diagnostics.Debug.WriteLine($"Error ensuring database created: {ex.Message}");
            }
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            await EnsureDatabaseCreatedAsync();
            
            var vehicles = new List<Vehicle>();
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, BranchId, Name, Description, VIN, LicensePlate, Make, Model, Year, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_Vehicles " +
                    "WHERE TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@TenantId", tenantId);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            vehicles.Add(MapVehicle(reader));
                        }
                    }
                }
            }
            
            return vehicles;
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<Vehicle>> GetByBranchIdAsync(int branchId)
        {
            await EnsureDatabaseCreatedAsync();
            
            var vehicles = new List<Vehicle>();
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, BranchId, Name, Description, VIN, LicensePlate, Make, Model, Year, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_Vehicles " +
                    "WHERE BranchId = @BranchId AND TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@BranchId", branchId);
                    command.Parameters.AddWithValue("@TenantId", tenantId);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            vehicles.Add(MapVehicle(reader));
                        }
                    }
                }
            }
            
            return vehicles;
        }
        
        /// <inheritdoc/>
        public async Task<Vehicle> GetByIdAsync(int id)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, BranchId, Name, Description, VIN, LicensePlate, Make, Model, Year, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_Vehicles " +
                    "WHERE Id = @Id AND TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@TenantId", tenantId);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapVehicle(reader);
                        }
                    }
                }
            }
            
            return null!;
        }
        
        /// <inheritdoc/>
        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            vehicle.TenantId = tenantId;
            vehicle.CreatedAt = DateTime.Now;
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "INSERT INTO TBL_Vehicles (BranchId, Name, Description, VIN, LicensePlate, Make, Model, Year, Status, TenantId, CreatedAt, CreatedBy) " +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@BranchId, @Name, @Description, @VIN, @LicensePlate, @Make, @Model, @Year, @Status, @TenantId, @CreatedAt, @CreatedBy)", connection))
                {
                    command.Parameters.AddWithValue("@BranchId", vehicle.BranchId);
                    command.Parameters.AddWithValue("@Name", vehicle.Name);
                    command.Parameters.AddWithValue("@Description", (object?)vehicle.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@VIN", (object?)vehicle.VIN ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LicensePlate", (object?)vehicle.LicensePlate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Make", (object?)vehicle.Make ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Model", (object?)vehicle.Model ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Year", (object?)vehicle.Year ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", vehicle.Status ?? "Y");
                    command.Parameters.AddWithValue("@TenantId", vehicle.TenantId);
                    command.Parameters.AddWithValue("@CreatedAt", vehicle.CreatedAt);
                    command.Parameters.AddWithValue("@CreatedBy", (object?)vehicle.CreatedBy ?? DBNull.Value);
                    
                    vehicle.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
            
            return vehicle;
        }
        
        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(Vehicle vehicle)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            vehicle.TenantId = tenantId;
            vehicle.UpdatedAt = DateTime.Now;
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "UPDATE TBL_Vehicles " +
                    "SET BranchId = @BranchId, Name = @Name, Description = @Description, " +
                    "VIN = @VIN, LicensePlate = @LicensePlate, Make = @Make, Model = @Model, Year = @Year, " +
                    "Status = @Status, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy " +
                    "WHERE Id = @Id AND TenantId = @TenantId", connection))
                {
                    command.Parameters.AddWithValue("@Id", vehicle.Id);
                    command.Parameters.AddWithValue("@BranchId", vehicle.BranchId);
                    command.Parameters.AddWithValue("@Name", vehicle.Name);
                    command.Parameters.AddWithValue("@Description", (object?)vehicle.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@VIN", (object?)vehicle.VIN ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LicensePlate", (object?)vehicle.LicensePlate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Make", (object?)vehicle.Make ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Model", (object?)vehicle.Model ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Year", (object?)vehicle.Year ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", vehicle.Status ?? "Y");
                    command.Parameters.AddWithValue("@TenantId", vehicle.TenantId);
                    command.Parameters.AddWithValue("@UpdatedAt", vehicle.UpdatedAt);
                    command.Parameters.AddWithValue("@UpdatedBy", (object?)vehicle.UpdatedBy ?? DBNull.Value);
                    
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
        
        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                // Soft delete by setting Status to 'D'
                using (var command = new SqlCommand(
                    "UPDATE TBL_Vehicles " +
                    "SET Status = 'D', UpdatedAt = @UpdatedAt " +
                    "WHERE Id = @Id AND TenantId = @TenantId", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@TenantId", tenantId);
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
        
        private Vehicle MapVehicle(SqlDataReader reader)
        {
            return new Vehicle
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                BranchId = reader.GetInt32(reader.GetOrdinal("BranchId")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                VIN = reader.IsDBNull(reader.GetOrdinal("VIN")) ? null : reader.GetString(reader.GetOrdinal("VIN")),
                LicensePlate = reader.IsDBNull(reader.GetOrdinal("LicensePlate")) ? null : reader.GetString(reader.GetOrdinal("LicensePlate")),
                Make = reader.IsDBNull(reader.GetOrdinal("Make")) ? null : reader.GetString(reader.GetOrdinal("Make")),
                Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader.GetString(reader.GetOrdinal("Model")),
                Year = reader.IsDBNull(reader.GetOrdinal("Year")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Year")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                TenantId = reader.GetInt32(reader.GetOrdinal("TenantId")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                UpdatedBy = reader.IsDBNull(reader.GetOrdinal("UpdatedBy")) ? null : reader.GetString(reader.GetOrdinal("UpdatedBy"))
            };
        }
    }
}