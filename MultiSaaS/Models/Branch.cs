using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiSaaS.Models
{
    /// <summary>
    /// Represents a branch in the system.
    /// This is the third level of the hierarchical model, under Company.
    /// </summary>
    public class Branch
    {
        /// <summary>
        /// The unique identifier for the branch.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// The company this branch belongs to.
        /// </summary>
        [Required]
        public int CompanyId { get; set; }
        
        /// <summary>
        /// Navigation property for the company.
        /// </summary>
        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }
        
        /// <summary>
        /// The name of the branch.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        /// <summary>
        /// The description of the branch.
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }
        
        /// <summary>
        /// The contact email for the branch.
        /// </summary>
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        /// <summary>
        /// The contact phone number for the branch.
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }
        
        /// <summary>
        /// The address of the branch.
        /// </summary>
        [StringLength(200)]
        public string? Address { get; set; }
        
        /// <summary>
        /// The status of the branch.
        /// Y = Active, N = Inactive, D = Deleted
        /// </summary>
        [StringLength(1)]
        public string Status { get; set; } = "Y";
        
        /// <summary>
        /// The tenant ID associated with this branch.
        /// </summary>
        [Required]
        public int TenantId { get; set; }
        
        /// <summary>
        /// The date when the branch was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        /// <summary>
        /// The user who created the branch.
        /// </summary>
        [StringLength(50)]
        public string? CreatedBy { get; set; }
        
        /// <summary>
        /// The date when the branch was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// The user who last updated the branch.
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