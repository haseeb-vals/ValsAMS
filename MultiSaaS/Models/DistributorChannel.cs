using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiSaaS.Models
{
    /// <summary>
    /// Represents a channel associated with a distributor in the system.
    /// </summary>
    public class DistributorChannel
    {
        /// <summary>
        /// The unique identifier for the distributor channel.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// The distributor this channel belongs to.
        /// </summary>
        [Required]
        public int DistributorId { get; set; }
        
        /// <summary>
        /// Navigation property for the distributor.
        /// </summary>
        [ForeignKey("DistributorId")]
        public Distributor? Distributor { get; set; }
        
        /// <summary>
        /// The name of the channel.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        /// <summary>
        /// The code assigned to the channel.
        /// </summary>
        [StringLength(50)]
        public string? Code { get; set; }
        
        /// <summary>
        /// The description of the channel.
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }
        
        /// <summary>
        /// The type of the channel (e.g., Retail, Wholesale, Online).
        /// </summary>
        [StringLength(50)]
        public string? Type { get; set; }
        
        /// <summary>
        /// The status of the channel (Active, Inactive, etc.).
        /// </summary>
        [StringLength(20)]
        public string? Status { get; set; }
        
        /// <summary>
        /// The date and time when the channel was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// The user who created the channel.
        /// </summary>
        [StringLength(50)]
        public string? CreatedBy { get; set; }
        
        /// <summary>
        /// The date and time when the channel was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// The user who last updated the channel.
        /// </summary>
        [StringLength(50)]
        public string? UpdatedBy { get; set; }
    }
}