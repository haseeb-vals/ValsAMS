using System;
using System.Collections.Generic;
using MultiSaaS.Models;

namespace MultiSaaS.ViewModels
{
    /// <summary>
    /// View model for displaying sub-distributor information.
    /// </summary>
    public class SubDistributorViewModel
    {
        /// <summary>
        /// The sub-distributor information.
        /// </summary>
        public SubDistributor? SubDistributor { get; set; }
        
        /// <summary>
        /// The parent distributor information.
        /// </summary>
        public Distributor? ParentDistributor { get; set; }
    }
}