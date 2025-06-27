using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiSaaS.Models
{
    /// <summary>
    /// Represents a user in the system.
    /// This is the fifth level of the hierarchical model, under Vehicle.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// The vehicle this user is associated with.
        /// </summary>
        public int? VehicleId { get; set; }
        
        /// <summary>
        /// Navigation property for the vehicle.
        /// </summary>
        [ForeignKey("VehicleId")]
        public Vehicle? Vehicle { get; set; }
        
        /// <summary>
        /// The username for the user.
        /// </summary>
        [StringLength(50)]
        public string? Username { get; set; }
        
        /// <summary>
        /// The email address of the user.
        /// </summary>
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        /// <summary>
        /// The first name of the user.
        /// </summary>
        [StringLength(50)]
        public string? FirstName { get; set; }
        
        /// <summary>
        /// The last name of the user.
        /// </summary>
        [StringLength(50)]
        public string? LastName { get; set; }
        
        /// <summary>
        /// The full name of the user (calculated from first and last name).
        /// </summary>
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();
        
        /// <summary>
        /// The phone number of the user.
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }
        
        /// <summary>
        /// The role of the user in the system.
        /// </summary>
        [StringLength(50)]
        public string? Role { get; set; }
        
        /// <summary>
        /// The status of the user.
        /// Y = Active, N = Inactive, D = Deleted
        /// </summary>
        [StringLength(1)]
        public string Status { get; set; } = "Y";
        
        /// <summary>
        /// The tenant ID associated with this user.
        /// </summary>
        [Required]
        public int TenantId { get; set; }
        
        /// <summary>
        /// The date when the user was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        /// <summary>
        /// The user who created this user.
        /// </summary>
        [StringLength(50)]
        public string? CreatedBy { get; set; }
        
        /// <summary>
        /// The date when the user was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// The user who last updated this user.
        /// </summary>
        [StringLength(50)]
        public string? UpdatedBy { get; set; }
        
        /// <summary>
        /// Boolean property for UI binding (true = Active, false = Inactive/Deleted)
        /// </summary>
        [NotMapped]
        public bool IsActive
        {
            get => Status == "Y";
            set => Status = value ? "Y" : "N";
        }
    }
}