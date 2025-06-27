using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiSaaS.Models
{
    /// <summary>
    /// Represents an organization in the system.
    /// This is the top level of the hierarchical model.
    /// </summary>
    public class Organization
    {
        /// <summary>
        /// The unique identifier for the organization.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// The name of the organization.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        /// <summary>
        /// The description of the organization.
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }
        
        /// <summary>
        /// The contact email for the organization.
        /// </summary>
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        /// <summary>
        /// The contact phone number for the organization.
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }
        
        /// <summary>
        /// The address of the organization.
        /// </summary>
        [StringLength(200)]
        public string? Address { get; set; }
        
        /// <summary>
        /// The status of the organization.
        /// Y = Active, N = Inactive, D = Deleted
        /// </summary>
        [StringLength(1)]
        public string Status { get; set; } = "Y";
        
        /// <summary>
        /// The tenant ID associated with this organization.
        /// </summary>
        [Required]
        public int TenantId { get; set; }
        
        /// <summary>
        /// The date when the organization was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        /// <summary>
        /// The user who created the organization.
        /// </summary>
        [StringLength(50)]
        public string? CreatedBy { get; set; }
        
        /// <summary>
        /// The date when the organization was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// The user who last updated the organization.
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