using System;
using MultiSaaS.Models;

namespace MultiSaaS.ViewModels
{
    /// <summary>
    /// View model for displaying sub-user information.
    /// </summary>
    public class SubUserViewModel
    {
        /// <summary>
        /// The unique identifier for the sub-user.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The user this sub-user is associated with.
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Navigation property for the user.
        /// </summary>
        public User? User { get; set; }
        
        /// <summary>
        /// The username for the sub-user.
        /// </summary>
        public string? Username { get; set; }
        
        /// <summary>
        /// The email address of the sub-user.
        /// </summary>
        public string? Email { get; set; }
        
        /// <summary>
        /// The first name of the sub-user.
        /// </summary>
        public string? FirstName { get; set; }
        
        /// <summary>
        /// The last name of the sub-user.
        /// </summary>
        public string? LastName { get; set; }
        
        /// <summary>
        /// The full name of the sub-user (calculated from first and last name).
        /// </summary>
        public string FullName => $"{FirstName} {LastName}".Trim();
        
        /// <summary>
        /// The phone number of the sub-user.
        /// </summary>
        public string? Phone { get; set; }
        
        /// <summary>
        /// The role of the sub-user in the system.
        /// </summary>
        public string? Role { get; set; }
        
        /// <summary>
        /// The status of the sub-user.
        /// </summary>
        public string Status { get; set; } = "Y";
        
        /// <summary>
        /// The date when the sub-user was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// The date when the sub-user was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// Boolean property for UI binding (true = Active, false = Inactive/Deleted)
        /// </summary>
        public bool IsActive
        {
            get => Status == "Y";
            set => Status = value ? "Y" : "N";
        }
    }
}