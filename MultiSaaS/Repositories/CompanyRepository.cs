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
    /// Repository implementation for companies.
    /// Uses ADO.NET to interact with the database.
    /// </summary>
    public class CompanyRepository : ICompanyRepository
    {
        private readonly string? _connectionString;
        private readonly ITenantProvider _tenantProvider;
        
        public CompanyRepository(IConfiguration configuration, ITenantProvider tenantProvider)
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
                        "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TBL_Companies') " +
                        "SELECT 1 ELSE SELECT 0", connection))
                    {
                        var tableExists = (Convert.ToInt32(await command.ExecuteScalarAsync()) == 1);
                        
                        // Only try to create the table if it doesn't exist
                        if (!tableExists)
                        {
                            try
                            {
                                using (var createCommand = new SqlCommand(
                                    "CREATE TABLE TBL_Companies (" +
                                    "Id INT IDENTITY(1,1) PRIMARY KEY, " +
                                    "OrganizationId INT NOT NULL, " +
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
                                    "UpdatedBy NVARCHAR(50), " +
                                    "CONSTRAINT FK_Companies_Organizations FOREIGN KEY (OrganizationId) " +
                                    "REFERENCES TBL_Organizations(Id)" +
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
        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            await EnsureDatabaseCreatedAsync();
            
            var companies = new List<Company>();
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, OrganizationId, Name, Description, Email, Phone, Address, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_Companies " +
                    "WHERE TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.Add("@TenantId", SqlDbType.Int).Value = tenantId;
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            companies.Add(MapCompany(reader));
                        }
                    }
                }
            }
            
            return companies;
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<Company>> GetByOrganizationIdAsync(int organizationId)
        {
            await EnsureDatabaseCreatedAsync();
            
            var companies = new List<Company>();
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, OrganizationId, Name, Description, Email, Phone, Address, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_Companies " +
                    "WHERE OrganizationId = @OrganizationId AND TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@OrganizationId", organizationId);
                    command.Parameters.Add("@TenantId", SqlDbType.Int).Value = tenantId;
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            companies.Add(MapCompany(reader));
                        }
                    }
                }
            }
            
            return companies;
        }
        
        /// <inheritdoc/>
        public async Task<Company> GetByIdAsync(int id)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, OrganizationId, Name, Description, Email, Phone, Address, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_Companies " +
                    "WHERE Id = @Id AND TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.Add("@TenantId", SqlDbType.Int).Value = tenantId;
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapCompany(reader);
                        }
                    }
                }
            }
            
            return null!;
        }
        
        /// <inheritdoc/>
        public async Task<Company> CreateAsync(Company company)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            company.TenantId = tenantId;
            company.CreatedAt = DateTime.Now;
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "INSERT INTO TBL_Companies (OrganizationId, Name, Description, Email, Phone, Address, Status, TenantId, CreatedAt, CreatedBy) " +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@OrganizationId, @Name, @Description, @Email, @Phone, @Address, @Status, @TenantId, @CreatedAt, @CreatedBy)", connection))
                {
                    command.Parameters.AddWithValue("@OrganizationId", company.OrganizationId);
                    command.Parameters.AddWithValue("@Name", company.Name);
                    command.Parameters.AddWithValue("@Description", (object?)company.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object?)company.Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object?)company.Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Address", (object?)company.Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", company.Status ?? "Y");
                    command.Parameters.AddWithValue("@TenantId", company.TenantId);
                    command.Parameters.AddWithValue("@CreatedAt", company.CreatedAt);
                    command.Parameters.AddWithValue("@CreatedBy", (object?)company.CreatedBy ?? DBNull.Value);
                    
                    company.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
            
            return company;
        }
        
        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(Company company)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            company.TenantId = tenantId;
            company.UpdatedAt = DateTime.Now;
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "UPDATE TBL_Companies " +
                    "SET OrganizationId = @OrganizationId, Name = @Name, Description = @Description, " +
                    "Email = @Email, Phone = @Phone, Address = @Address, Status = @Status, " +
                    "UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy " +
                    "WHERE Id = @Id AND TenantId = @TenantId", connection))
                {
                    command.Parameters.AddWithValue("@Id", company.Id);
                    command.Parameters.AddWithValue("@OrganizationId", company.OrganizationId);
                    command.Parameters.AddWithValue("@Name", company.Name);
                    command.Parameters.AddWithValue("@Description", (object?)company.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object?)company.Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object?)company.Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Address", (object?)company.Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", company.Status ?? "Y");
                    command.Parameters.AddWithValue("@TenantId", company.TenantId);
                    command.Parameters.AddWithValue("@UpdatedAt", company.UpdatedAt);
                    command.Parameters.AddWithValue("@UpdatedBy", (object?)company.UpdatedBy ?? DBNull.Value);
                    
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
                    "UPDATE TBL_Companies " +
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
        
        private Company MapCompany(SqlDataReader reader)
        {
            return new Company
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                OrganizationId = reader.GetInt32(reader.GetOrdinal("OrganizationId")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                TenantId = reader.GetInt32(8),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                UpdatedBy = reader.IsDBNull(reader.GetOrdinal("UpdatedBy")) ? null : reader.GetString(reader.GetOrdinal("UpdatedBy"))
            };
        }
    }
}