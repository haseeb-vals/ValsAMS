using System;
using System.Collections.Generic;
using System.Linq;
using MultiSaaS.Models;

namespace MultiSaaS.ViewModels
{
    /// <summary>
    /// View model for displaying distributor information.
    /// </summary>
    public class DistributorViewModel
    {
        /// <summary>
        /// The distributor information.
        /// </summary>
        public Distributor? Distributor { get; set; }
        
        /// <summary>
        /// The list of channels associated with the distributor.
        /// </summary>
        public List<DistributorChannel>? Channels { get; set; }
        
        /// <summary>
        /// The list of payment terms associated with the distributor.
        /// </summary>
        public List<DistributorPaymentTerm>? PaymentTerms { get; set; }
        
        /// <summary>
        /// The list of sub-distributors associated with the distributor.
        /// </summary>
        public List<SubDistributor>? SubDistributors { get; set; }
        
        /// <summary>
        /// The list of devices associated with the distributor.
        /// </summary>
        public List<Device>? Devices { get; set; }
        
        /// <summary>
        /// The count of testing devices currently assigned.
        /// </summary>
        public int AssignedTestingDevicesCount => Devices?.Count(d => d.IsTestingDevice) ?? 0;
        
        /// <summary>
        /// The count of regular devices currently assigned.
        /// </summary>
        public int AssignedDevicesCount => Devices?.Count(d => !d.IsTestingDevice) ?? 0;
    }
}