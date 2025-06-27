using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiSaaS.Models
{
    /// <summary>
    /// Represents a sub-user in the system.
    /// This is the sixth level of the hierarchical model, under User.
    /// </summary>
    public class SubUser
    {
        /// <summary>
        /// The unique identifier for the sub-user.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// The user this sub-user is associated with.
        /// </summary>
        [Required]
        public int UserId { get; set; }
        
        /// <summary>
        /// Navigation property for the user.
        /// </summary>
        [ForeignKey("UserId")]
        public User? User { get; set; }
        
        /// <summary>
        /// The username for the sub-user.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string? Username { get; set; }
        
        /// <summary>
        /// The email address of the sub-user.
        /// </summary>
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        /// <summary>
        /// The first name of the sub-user.
        /// </summary>
        [StringLength(50)]
        public string? FirstName { get; set; }
        
        /// <summary>
        /// The last name of the sub-user.
        /// </summary>
        [StringLength(50)]
        public string? LastName { get; set; }
        
        /// <summary>
        /// The full name of the sub-user (calculated from first and last name).
        /// </summary>
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();
        
        /// <summary>
        /// The phone number of the sub-user.
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }
        
        /// <summary>
        /// The role of the sub-user in the system.
        /// </summary>
        [StringLength(50)]
        public string? Role { get; set; }
        
        /// <summary>
        /// The status of the sub-user.
        /// Y = Active, N = Inactive, D = Deleted
        /// </summary>
        [StringLength(1)]
        public string Status { get; set; } = "Y";
        
        /// <summary>
        /// The tenant ID associated with this sub-user.
        /// </summary>
        [Required]
        public int TenantId { get; set; }
        
        /// <summary>
        /// The date when the sub-user was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        /// <summary>
        /// The user who created this sub-user.
        /// </summary>
        [StringLength(50)]
        public string? CreatedBy { get; set; }
        
        /// <summary>
        /// The date when the sub-user was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// The user who last updated this sub-user.
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