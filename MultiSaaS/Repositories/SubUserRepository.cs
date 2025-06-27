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
    /// Repository implementation for subusers.
    /// Uses ADO.NET to interact with the database.
    /// </summary>
    public class SubUserRepository : ISubUserRepository
    {
        private readonly string? _connectionString;
        private readonly ITenantProvider _tenantProvider;
        
        public SubUserRepository(IConfiguration configuration, ITenantProvider tenantProvider)
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
                        "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TBL_SubUsers') " +
                        "SELECT 1 ELSE SELECT 0", connection))
                    {
                        var tableExists = (Convert.ToInt32(await command.ExecuteScalarAsync()) == 1);
                        
                        // Only try to create the table if it doesn't exist
                        if (!tableExists)
                        {
                            try
                            {
                                using (var createCommand = new SqlCommand(
                                    "CREATE TABLE TBL_SubUsers (" +
                                    "Id INT IDENTITY(1,1) PRIMARY KEY, " +
                                    "UserId INT NOT NULL, " +
                                    "Username NVARCHAR(50) NOT NULL, " +
                                    "Email NVARCHAR(100) NOT NULL, " +
                                    "FirstName NVARCHAR(50), " +
                                    "LastName NVARCHAR(50), " +
                                    "Phone NVARCHAR(20), " +
                                    "Role NVARCHAR(20), " +
                                    "Status NVARCHAR(1) DEFAULT 'Y', " +
                                    "TenantId INT NOT NULL, " + // was NVARCHAR(50)
                                    "CreatedAt DATETIME DEFAULT GETDATE(), " +
                                    "CreatedBy NVARCHAR(50), " +
                                    "UpdatedAt DATETIME, " +
                                    "UpdatedBy NVARCHAR(50), " +
                                    "CONSTRAINT FK_SubUsers_Users FOREIGN KEY (UserId) " +
                                    "REFERENCES TBL_Users(Id)" +
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
        public async Task<IEnumerable<SubUser>> GetAllAsync()
        {
            await EnsureDatabaseCreatedAsync();
            
            var subUsers = new List<SubUser>();
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, UserId, Username, Email, FirstName, LastName, Phone, Role, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_SubUsers " +
                    "WHERE TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.Add("@TenantId", SqlDbType.Int).Value = tenantId;
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            subUsers.Add(MapSubUser(reader));
                        }
                    }
                }
            }
            
            return subUsers;
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<SubUser>> GetByUserIdAsync(int userId)
        {
            await EnsureDatabaseCreatedAsync();
            
            var subUsers = new List<SubUser>();
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, UserId, Username, Email, FirstName, LastName, Phone, Role, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_SubUsers " +
                    "WHERE UserId = @UserId AND TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.Add("@TenantId", SqlDbType.Int).Value = tenantId;
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            subUsers.Add(MapSubUser(reader));
                        }
                    }
                }
            }
            
            return subUsers;
        }
        
        /// <inheritdoc/>
        public async Task<SubUser> GetByIdAsync(int id)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, UserId, Username, Email, FirstName, LastName, Phone, Role, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_SubUsers " +
                    "WHERE Id = @Id AND TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.Add("@TenantId", SqlDbType.Int).Value = tenantId;
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapSubUser(reader);
                        }
                    }
                }
            }
            
            return null!;
        }
        
        /// <inheritdoc/>
        public async Task<SubUser> GetByUsernameAsync(string username)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, UserId, Username, Email, FirstName, LastName, Phone, Role, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_SubUsers " +
                    "WHERE Username = @Username AND TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.Add("@TenantId", SqlDbType.Int).Value = tenantId;
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapSubUser(reader);
                        }
                    }
                }
            }
            
            return null!;
        }
        
        /// <inheritdoc/>
        public async Task<SubUser> GetByEmailAsync(string email)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "SELECT Id, UserId, Username, Email, FirstName, LastName, Phone, Role, Status, TenantId, " +
                    "CreatedAt, CreatedBy, UpdatedAt, UpdatedBy " +
                    "FROM TBL_SubUsers " +
                    "WHERE Email = @Email AND TenantId = @TenantId AND Status <> 'D'", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.Add("@TenantId", SqlDbType.Int).Value = tenantId;
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapSubUser(reader);
                        }
                    }
                }
            }
            
            return null!;
        }
        
        /// <inheritdoc/>
        public async Task<SubUser> CreateAsync(SubUser subUser)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            subUser.TenantId = tenantId;
            subUser.CreatedAt = DateTime.Now;
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "INSERT INTO TBL_SubUsers (UserId, Username, Email, FirstName, LastName, Phone, Role, Status, TenantId, CreatedAt, CreatedBy) " +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@UserId, @Username, @Email, @FirstName, @LastName, @Phone, @Role, @Status, @TenantId, @CreatedAt, @CreatedBy)", connection))
                {
                    command.Parameters.AddWithValue("@UserId", subUser.UserId);
                    command.Parameters.AddWithValue("@Username", subUser.Username);
                    command.Parameters.AddWithValue("@Email", subUser.Email);
                    command.Parameters.AddWithValue("@FirstName", (object?)subUser.FirstName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", (object?)subUser.LastName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object?)subUser.Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Role", (object?)subUser.Role ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", subUser.Status ?? "Y");
                    command.Parameters.AddWithValue("@TenantId", subUser.TenantId);
                    command.Parameters.AddWithValue("@CreatedAt", subUser.CreatedAt);
                    command.Parameters.AddWithValue("@CreatedBy", (object?)subUser.CreatedBy ?? DBNull.Value);
                    
                    subUser.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
            
            return subUser;
        }
        
        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(SubUser subUser)
        {
            await EnsureDatabaseCreatedAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            subUser.TenantId = tenantId;
            subUser.UpdatedAt = DateTime.Now;
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new SqlCommand(
                    "UPDATE TBL_SubUsers " +
                    "SET UserId = @UserId, Username = @Username, Email = @Email, " +
                    "FirstName = @FirstName, LastName = @LastName, Phone = @Phone, Role = @Role, " +
                    "Status = @Status, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy " +
                    "WHERE Id = @Id AND TenantId = @TenantId", connection))
                {
                    command.Parameters.AddWithValue("@Id", subUser.Id);
                    command.Parameters.AddWithValue("@UserId", subUser.UserId);
                    command.Parameters.AddWithValue("@Username", subUser.Username);
                    command.Parameters.AddWithValue("@Email", subUser.Email);
                    command.Parameters.AddWithValue("@FirstName", (object?)subUser.FirstName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", (object?)subUser.LastName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object?)subUser.Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Role", (object?)subUser.Role ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", subUser.Status ?? "Y");
                    command.Parameters.AddWithValue("@TenantId", subUser.TenantId);
                    command.Parameters.AddWithValue("@UpdatedAt", subUser.UpdatedAt);
                    command.Parameters.AddWithValue("@UpdatedBy", (object?)subUser.UpdatedBy ?? DBNull.Value);
                    
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
                    "UPDATE TBL_SubUsers " +
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
        
        private SubUser MapSubUser(SqlDataReader reader)
        {
            return new SubUser
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                Role = reader.IsDBNull(reader.GetOrdinal("Role")) ? null : reader.GetString(reader.GetOrdinal("Role")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                TenantId = reader.GetInt32(9), // was reader.GetString(reader.GetOrdinal("TenantId"))
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                UpdatedBy = reader.IsDBNull(reader.GetOrdinal("UpdatedBy")) ? null : reader.GetString(reader.GetOrdinal("UpdatedBy"))
            };
        }
    }
}