using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiSaaS.Models
{
    /// <summary>
    /// Represents a distributor in the system.
    /// This is a parallel hierarchy to the Organization-Company-Branch model.
    /// </summary>
    public class Distributor
    {
        /// <summary>
        /// The unique identifier for the distributor.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// The name of the distributor.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        /// <summary>
        /// The code assigned to the distributor.
        /// </summary>
        [StringLength(50)]
        public string? Code { get; set; }
        
        /// <summary>
        /// The description of the distributor.
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }
        
        /// <summary>
        /// The contact email for the distributor.
        /// </summary>
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        /// <summary>
        /// The contact phone number for the distributor.
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }
        
        /// <summary>
        /// The address of the distributor.
        /// </summary>
        [StringLength(200)]
        public string? Address { get; set; }
        
        /// <summary>
        /// The city of the distributor.
        /// </summary>
        [StringLength(50)]
        public string? City { get; set; }
        
        /// <summary>
        /// The state/province of the distributor.
        /// </summary>
        [StringLength(50)]
        public string? State { get; set; }
        
        /// <summary>
        /// The country of the distributor.
        /// </summary>
        [StringLength(50)]
        public string? Country { get; set; }
        
        /// <summary>
        /// The postal/zip code of the distributor.
        /// </summary>
        [StringLength(20)]
        public string? PostalCode { get; set; }
        
        /// <summary>
        /// The website URL of the distributor.
        /// </summary>
        [StringLength(100)]
        public string? Website { get; set; }
        
        /// <summary>
        /// The status of the distributor (Active, Inactive, etc.).
        /// </summary>
        [StringLength(20)]
        public string? Status { get; set; }
        
        /// <summary>
        /// The Service Level Agreement number for this distributor.
        /// </summary>
        [StringLength(20)]
        public string? SlaNumber { get; set; }
        
        /// <summary>
        /// The billing flag indicating the billing cycle (M: Monthly, Q: Quarterly, S: Semi-Annual, Y: Annual).
        /// </summary>
        [StringLength(1)]
        public string? BillingFlag { get; set; }
        
        /// <summary>
        /// The number of days allowed for payment after billing.
        /// </summary>
        public int? DueDays { get; set; }
        
        /// <summary>
        /// The percentage penalty applied for overdue payments.
        /// </summary>
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? OverduePercentage { get; set; }
        
        /// <summary>
        /// The maximum number of sub-distributors allowed for this distributor.
        /// </summary>
        public int MaxSubDistributionAllowed { get; set; } = 5;
        
        /// <summary>
        /// The maximum number of testing devices allowed for this distributor.
        /// </summary>
        public int MaxTestingDeviceAllowed { get; set; } = 10;
        
        /// <summary>
        /// The maximum number of devices allowed for this distributor.
        /// </summary>
        public int MaxDeviceAllowed { get; set; } = 100;
        
        /// <summary>
        /// The date and time when the distributor was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// The user who created the distributor.
        /// </summary>
        [StringLength(50)]
        public string? CreatedBy { get; set; }
        
        /// <summary>
        /// The date and time when the distributor was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// The user who last updated the distributor.
        /// </summary>
        [StringLength(50)]
        public string? UpdatedBy { get; set; }
    }
}