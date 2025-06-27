using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiSaaS.Models
{
    /// <summary>
    /// Represents a company in the system.
    /// This is the second level of the hierarchical model, under Organization.
    /// </summary>
    public class Company
    {
        /// <summary>
        /// The unique identifier for the company.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// The organization this company belongs to.
        /// </summary>
        [Required]
        public int OrganizationId { get; set; }
        
        /// <summary>
        /// Navigation property for the organization.
        /// </summary>
        [ForeignKey("OrganizationId")]
        public Organization? Organization { get; set; }
        
        /// <summary>
        /// The name of the company.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        /// <summary>
        /// The description of the company.
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }
        
        /// <summary>
        /// The contact email for the company.
        /// </summary>
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        /// <summary>
        /// The contact phone number for the company.
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }
        
        /// <summary>
        /// The address of the company.
        /// </summary>
        [StringLength(200)]
        public string? Address { get; set; }
        
        /// <summary>
        /// The status of the company.
        /// Y = Active, N = Inactive, D = Deleted
        /// </summary>
        [StringLength(1)]
        public string Status { get; set; } = "Y";
        
        /// <summary>
        /// The tenant ID associated with this company.
        /// </summary>
        [Required]
        public int TenantId { get; set; }
        
        /// <summary>
        /// The date when the company was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        /// <summary>
        /// The user who created the company.
        /// </summary>
        [StringLength(50)]
        public string? CreatedBy { get; set; }
        
        /// <summary>
        /// The date when the company was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// The user who last updated the company.
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