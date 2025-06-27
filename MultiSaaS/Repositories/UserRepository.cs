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
    /// Repository implementation for users.
    /// Uses ADO.NET to interact with the database.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly string? _connectionString;
        private readonly ITenantProvider _tenantProvider;
        
        public UserRepository(IConfiguration configuration, ITenantProvider tenantProvider)
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
                        "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TBL_Users') " +
                        "SELECT 1 ELSE SELECT 0", connection))
                    {
                        var tableExists = (Convert.ToInt32(await command.ExecuteScalarAsync()) == 1);
                        
                        // Only try to create the table if it doesn't exist
                        if (!tableExists)
                        {
                            try
                            {
                                using (var createCommand = new SqlCommand(
                                    "CREATE TABLE TBL_Users (" +
                                    "Id INT IDENTITY(1,1) PRIMARY KEY, " +
                                    "VehicleId INT NOT NULL, " +
                                    "Username NVARCHAR(50) NOT NULL, " +
                                    "Email NVARCHAR(100) NOT NULL, " +
                                    "FirstName NVARCHAR(50), " +
                                    "LastName NVARCHAR(50), " +
                                    "Phone NVARCHAR(20), " +
                                    "Role NVARCHAR(20), " +
                                    "Status NVARCHAR(1) DEFAULT 'Y', " +
                                    "TenantId NVARCHAR(50) NOT NULL, " +
                                    "CreatedAt DATETIME DEFAULT GETDATE(), " +
                                    "CreatedBy NVARCHAR(50), " +
                                    "UpdatedAt DATETIME, " +
                                    "UpdatedBy NVARCHAR(50), " +
                                    "CONSTRAINT FK_Users_Vehicles FOREIGN KEY (VehicleId) " +
                                    "REFERENCES TBL_Vehicles(Id)" +
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
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            await EnsureDatabaseCreatedAsync();
            
            var users = new List<User>();
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, VehicleId, Username, Email, FirstName, LastName, Phone, Role, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_Users " +
                    "WHERE TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@TenantId", tenantId);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(MapUser(reader));
                        }
                    }
                }
            }
            
            return users;
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetByVehicleIdAsync(int vehicleId)
        {
            await EnsureDatabaseCreatedAsync();
            
            var users = new List<User>();
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, VehicleId, Username, Email, FirstName, LastName, Phone, Role, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_Users " +
                    "WHERE VehicleId = @VehicleId AND TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@VehicleId", vehicleId);
                    command.Parameters.AddWithValue("@TenantId", tenantId);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(MapUser(reader));
                        }
                    }
                }
            }
            
            return users;
        }
        
        /// <inheritdoc/>
        public async Task<User> GetByIdAsync(int id)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, VehicleId, Username, Email, FirstName, LastName, Phone, Role, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_Users " +
                    "WHERE Id = @Id AND TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@TenantId", tenantId);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapUser(reader);
                        }
                    }
                }
            }
            
            return null!;
        }
        
        /// <inheritdoc/>
        public async Task<User> GetByUsernameAsync(string username)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, VehicleId, Username, Email, FirstName, LastName, Phone, Role, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_Users " +
                    "WHERE Username = @Username AND TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@TenantId", tenantId);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapUser(reader);
                        }
                    }
                }
            }
            
            return null!;
        }
        
        /// <inheritdoc/>
        public async Task<User> GetByEmailAsync(string email)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, VehicleId, Username, Email, FirstName, LastName, Phone, Role, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_Users " +
                    "WHERE Email = @Email AND TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@TenantId", tenantId);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapUser(reader);
                        }
                    }
                }
            }
            
            return null!;
        }
        
        /// <inheritdoc/>
        public async Task<User> CreateAsync(User user)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            user.TenantId = tenantId;
            user.CreatedAt = DateTime.Now;
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "INSERT INTO TBL_Users (VehicleId, Username, Email, FirstName, LastName, Phone, Role, Status, TenantId, CreatedAt, CreatedBy) " +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@VehicleId, @Username, @Email, @FirstName, @LastName, @Phone, @Role, @Status, @TenantId, @CreatedAt, @CreatedBy)", connection))
                {
                    command.Parameters.AddWithValue("@VehicleId", user.VehicleId);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@FirstName", (object?)user.FirstName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", (object?)user.LastName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object?)user.Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Role", (object?)user.Role ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", user.Status ?? "Y");
                    command.Parameters.AddWithValue("@TenantId", user.TenantId);
                    command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
                    command.Parameters.AddWithValue("@CreatedBy", (object?)user.CreatedBy ?? DBNull.Value);
                    
                    user.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
            
            return user;
        }
        
        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(User user)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            user.TenantId = tenantId;
            user.UpdatedAt = DateTime.Now;
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "UPDATE TBL_Users " +
                    "SET VehicleId = @VehicleId, Username = @Username, Email = @Email, " +
                    "FirstName = @FirstName, LastName = @LastName, Phone = @Phone, Role = @Role, " +
                    "Status = @Status, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy " +
                    "WHERE Id = @Id AND TenantId = @TenantId", connection))
                {
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@VehicleId", user.VehicleId);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@FirstName", (object?)user.FirstName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", (object?)user.LastName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object?)user.Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Role", (object?)user.Role ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", user.Status ?? "Y");
                    command.Parameters.AddWithValue("@TenantId", user.TenantId);
                    command.Parameters.AddWithValue("@UpdatedAt", user.UpdatedAt);
                    command.Parameters.AddWithValue("@UpdatedBy", (object?)user.UpdatedBy ?? DBNull.Value);
                    
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
                    "UPDATE TBL_Users " +
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
        
        private User MapUser(SqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                VehicleId = reader.GetInt32(reader.GetOrdinal("VehicleId")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                Role = reader.IsDBNull(reader.GetOrdinal("Role")) ? null : reader.GetString(reader.GetOrdinal("Role")),
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