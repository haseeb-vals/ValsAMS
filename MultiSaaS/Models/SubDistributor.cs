using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiSaaS.Models
{
    /// <summary>
    /// Represents a sub-distributor in the system.
    /// This is linked to a parent distributor.
    /// </summary>
    public class SubDistributor
    {
        /// <summary>
        /// The unique identifier for the sub-distributor.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// The distributor this sub-distributor belongs to.
        /// </summary>
        [Required]
        public int DistributorId { get; set; }
        
        /// <summary>
        /// Navigation property for the distributor.
        /// </summary>
        [ForeignKey("DistributorId")]
        public Distributor? Distributor { get; set; }
        
        /// <summary>
        /// The name of the sub-distributor.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        /// <summary>
        /// The code assigned to the sub-distributor.
        /// </summary>
        [StringLength(50)]
        public string? Code { get; set; }
        
        /// <summary>
        /// The description of the sub-distributor.
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }
        
        /// <summary>
        /// The contact email for the sub-distributor.
        /// </summary>
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        /// <summary>
        /// The contact phone number for the sub-distributor.
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }
        
        /// <summary>
        /// The address of the sub-distributor.
        /// </summary>
        [StringLength(200)]
        public string? Address { get; set; }
        
        /// <summary>
        /// The city of the sub-distributor.
        /// </summary>
        [StringLength(50)]
        public string? City { get; set; }
        
        /// <summary>
        /// The state/province of the sub-distributor.
        /// </summary>
        [StringLength(50)]
        public string? State { get; set; }
        
        /// <summary>
        /// The country of the sub-distributor.
        /// </summary>
        [StringLength(50)]
        public string? Country { get; set; }
        
        /// <summary>
        /// The postal/zip code of the sub-distributor.
        /// </summary>
        [StringLength(20)]
        public string? PostalCode { get; set; }
        
        /// <summary>
        /// The website URL of the sub-distributor.
        /// </summary>
        [StringLength(100)]
        public string? Website { get; set; }
        
        /// <summary>
        /// The status of the sub-distributor (Active, Inactive, etc.).
        /// </summary>
        [StringLength(20)]
        public string? Status { get; set; }
        
        /// <summary>
        /// The date and time when the sub-distributor was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// The user who created the sub-distributor.
        /// </summary>
        [StringLength(50)]
        public string? CreatedBy { get; set; }
        
        /// <summary>
        /// The date and time when the sub-distributor was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// The user who last updated the sub-distributor.
        /// </summary>
        [StringLength(50)]
        public string? UpdatedBy { get; set; }
    }
}