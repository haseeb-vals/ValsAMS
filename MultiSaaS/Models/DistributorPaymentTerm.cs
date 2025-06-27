using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiSaaS.Models
{
    /// <summary>
    /// Represents a payment term associated with a distributor in the system.
    /// </summary>
    public class DistributorPaymentTerm
    {
        /// <summary>
        /// The unique identifier for the distributor payment term.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// The distributor this payment term belongs to.
        /// </summary>
        [Required]
        public int DistributorId { get; set; }
        
        /// <summary>
        /// Navigation property for the distributor.
        /// </summary>
        [ForeignKey("DistributorId")]
        public Distributor? Distributor { get; set; }
        
        /// <summary>
        /// The name of the payment term.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        /// <summary>
        /// The code assigned to the payment term.
        /// </summary>
        [StringLength(50)]
        public string? Code { get; set; }
        
        /// <summary>
        /// The description of the payment term.
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }
        
        /// <summary>
        /// The duration of the payment term in days.
        /// </summary>
        public int? DurationDays { get; set; }
        
        /// <summary>
        /// The discount percentage for early payment.
        /// </summary>
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? DiscountPercentage { get; set; }
        
        /// <summary>
        /// The number of days for early payment discount eligibility.
        /// </summary>
        public int? DiscountDays { get; set; }
        
        /// <summary>
        /// The status of the payment term (Active, Inactive, etc.).
        /// </summary>
        [StringLength(20)]
        public string? Status { get; set; }
        
        /// <summary>
        /// The date and time when the payment term was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// The user who created the payment term.
        /// </summary>
        [StringLength(50)]
        public string? CreatedBy { get; set; }
        
        /// <summary>
        /// The date and time when the payment term was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// The user who last updated the payment term.
        /// </summary>
        [StringLength(50)]
        public string? UpdatedBy { get; set; }
    }
}