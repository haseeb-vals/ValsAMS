using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiSaaS.Models
{
    /// <summary>
    /// Represents a device in the system.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// The unique identifier for the device.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// The distributor this device belongs to.
        /// </summary>
        public int DistributorId { get; set; }
        
        /// <summary>
        /// Navigation property for the distributor.
        /// </summary>
        [ForeignKey("DistributorId")]
        public Distributor? Distributor { get; set; }
        
        /// <summary>
        /// The sub-distributor this device belongs to (if any).
        /// </summary>
        public int? SubDistributorId { get; set; }
        
        /// <summary>
        /// Navigation property for the sub-distributor.
        /// </summary>
        [ForeignKey("SubDistributorId")]
        public SubDistributor? SubDistributor { get; set; }
        
        /// <summary>
        /// The name of the device.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        /// <summary>
        /// The code/serial number of the device.
        /// </summary>
        [StringLength(50)]
        public string? Code { get; set; }
        
        /// <summary>
        /// The type of the device (e.g., Hardwired GPS, Battery Tracker, etc.).
        /// </summary>
        [StringLength(50)]
        public string? Type { get; set; }
        
        /// <summary>
        /// The model of the device.
        /// </summary>
        [StringLength(50)]
        public string? Model { get; set; }
        
        /// <summary>
        /// The IMEI number of the device.
        /// </summary>
        [StringLength(20)]
        public string? IMEI { get; set; }
        
        /// <summary>
        /// Indicates whether this is a testing device.
        /// </summary>
        public bool IsTestingDevice { get; set; }
        
        /// <summary>
        /// The status of the device (Active, Inactive, etc.).
        /// </summary>
        [StringLength(20)]
        public string? Status { get; set; }
        
        /// <summary>
        /// The date and time when the device was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// The user who created the device.
        /// </summary>
        [StringLength(50)]
        public string? CreatedBy { get; set; }
        
        /// <summary>
        /// The date and time when the device was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// The user who last updated the device.
        /// </summary>
        [StringLength(50)]
        public string? UpdatedBy { get; set; }
    }
}