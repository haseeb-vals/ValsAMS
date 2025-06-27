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
    /// Repository implementation for organizations.
    /// Uses ADO.NET to interact with the database.
    /// </summary>
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly string? _connectionString;
        private readonly ITenantProvider _tenantProvider;
        
        public OrganizationRepository(IConfiguration configuration, ITenantProvider tenantProvider)
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
                        "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TBL_Organizations') " +
                        "SELECT 1 ELSE SELECT 0", connection))
                    {
                        var tableExists = (Convert.ToInt32(await command.ExecuteScalarAsync()) == 1);
                        
                        // Only try to create the table if it doesn't exist
                        if (!tableExists)
                        {
                            try
                            {
                                using (var createCommand = new SqlCommand(
                                    "CREATE TABLE TBL_Organizations (" +
                                    "Id INT IDENTITY(1,1) PRIMARY KEY, " +
                                    "Name NVARCHAR(100) NOT NULL, " +
                                    "Description NVARCHAR(500), " +
                                    "Email NVARCHAR(100), " +
                                    "Phone NVARCHAR(20), " +
                                    "Address NVARCHAR(200), " +
                                    "Status NVARCHAR(1) DEFAULT 'Y', " +
                                    "TenantId INT NOT NULL, " +
                                    "CreatedAt DATETIME DEFAULT GETDATE(), " +
                                    "CreatedBy NVARCHAR(50), " +
                                    "UpdatedAt DATETIME, " +
                                    "UpdatedBy NVARCHAR(50)" +
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
        public async Task<IEnumerable<Organization>> GetAllAsync()
        {
            await EnsureDatabaseCreatedAsync();
            
            var organizations = new List<Organization>();
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, Name, Description, Email, Phone, Address, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_Organizations " +
                    "WHERE TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.Add("@TenantId", SqlDbType.Int).Value = tenantId;
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            organizations.Add(MapOrganization(reader));
                        }
                    }
                }
            }
            
            return organizations;
        }
        
        /// <inheritdoc/>
        public async Task<Organization> GetByIdAsync(int id)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, Name, Description, Email, Phone, Address, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_Organizations " +
                    "WHERE Id = @Id AND TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.Add("@TenantId", SqlDbType.Int).Value = tenantId;
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapOrganization(reader);
                        }
                    }
                }
            }
            
            return null!;
        }
        
        /// <inheritdoc/>
        public async Task<Organization> CreateAsync(Organization organization)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            organization.TenantId = tenantId;
            organization.CreatedAt = DateTime.Now;
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "INSERT INTO TBL_Organizations (Name, Description, Email, Phone, Address, Status, TenantId, CreatedAt, CreatedBy) " +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@Name, @Description, @Email, @Phone, @Address, @Status, @TenantId, @CreatedAt, @CreatedBy)", connection))
                {
                    command.Parameters.AddWithValue("@Name", organization.Name);
                    command.Parameters.AddWithValue("@Description", (object?)organization.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object?)organization.Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object?)organization.Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Address", (object?)organization.Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", organization.Status ?? "Y");
                    command.Parameters.AddWithValue("@TenantId", organization.TenantId);
                    command.Parameters.AddWithValue("@CreatedAt", organization.CreatedAt);
                    command.Parameters.AddWithValue("@CreatedBy", (object?)organization.CreatedBy ?? DBNull.Value);
                    
                    organization.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
            
            return organization;
        }
        
        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(Organization organization)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            organization.TenantId = tenantId;
            organization.UpdatedAt = DateTime.Now;
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "UPDATE TBL_Organizations " +
                    "SET Name = @Name, Description = @Description, Email = @Email, Phone = @Phone, " +
                    "Address = @Address, Status = @Status, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy " +
                    "WHERE Id = @Id AND TenantId = @TenantId", connection))
                {
                    command.Parameters.AddWithValue("@Id", organization.Id);
                    command.Parameters.AddWithValue("@Name", organization.Name);
                    command.Parameters.AddWithValue("@Description", (object?)organization.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object?)organization.Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object?)organization.Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Address", (object?)organization.Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", organization.Status ?? "Y");
                    command.Parameters.AddWithValue("@TenantId", organization.TenantId);
                    command.Parameters.AddWithValue("@UpdatedAt", organization.UpdatedAt);
                    command.Parameters.AddWithValue("@UpdatedBy", (object?)organization.UpdatedBy ?? DBNull.Value);
                    
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
                    "UPDATE TBL_Organizations " +
                    "SET Status = 'D', UpdatedAt = @UpdatedAt " +
                    "WHERE Id = @Id AND TenantId = @TenantId", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.Add("@TenantId", SqlDbType.Int).Value = tenantId;
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
        
        private Organization MapOrganization(SqlDataReader reader)
        {
            return new Organization
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
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