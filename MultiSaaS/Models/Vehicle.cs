using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiSaaS.Models
{
    /// <summary>
    /// Represents a vehicle in the system.
    /// This is the fourth level of the hierarchical model, under Branch.
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// The unique identifier for the vehicle.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// The branch this vehicle belongs to.
        /// </summary>
        [Required]
        public int BranchId { get; set; }
        
        /// <summary>
        /// Navigation property for the branch.
        /// </summary>
        [ForeignKey("BranchId")]
        public Branch? Branch { get; set; }
        
        /// <summary>
        /// The name or identifier of the vehicle.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        /// <summary>
        /// The description of the vehicle.
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }
        
        /// <summary>
        /// The vehicle identification number (VIN).
        /// </summary>
        [StringLength(50)]
        public string? VIN { get; set; }
        
        /// <summary>
        /// The license plate number of the vehicle.
        /// </summary>
        [StringLength(20)]
        public string? LicensePlate { get; set; }
        
        /// <summary>
        /// The make of the vehicle.
        /// </summary>
        [StringLength(50)]
        public string? Make { get; set; }
        
        /// <summary>
        /// The model of the vehicle.
        /// </summary>
        [StringLength(50)]
        public string? Model { get; set; }
        
        /// <summary>
        /// The year of the vehicle.
        /// </summary>
        public int? Year { get; set; }
        
        /// <summary>
        /// The status of the vehicle.
        /// Y = Active, N = Inactive, D = Deleted
        /// </summary>
        [StringLength(1)]
        public string Status { get; set; } = "Y";
        
        /// <summary>
        /// The tenant ID associated with this vehicle.
        /// </summary>
        [Required]
        public int TenantId { get; set; }
        
        /// <summary>
        /// The date when the vehicle was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        /// <summary>
        /// The user who created the vehicle.
        /// </summary>
        [StringLength(50)]
        public string? CreatedBy { get; set; }
        
        /// <summary>
        /// The date when the vehicle was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// The user who last updated the vehicle.
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