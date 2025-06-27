using System.Collections.Generic;
using MultiSaaS.Models;

namespace MultiSaaS.ViewModels
{
    public class DeviceViewModel
    {
        public required Device Device { get; set; }
        public required Distributor Distributor { get; set; }
        public required List<SubDistributor> SubDistributors { get; set; }
        public required List<string> DeviceTypes { get; set; }
        public bool IsTestingDevice { get; set; }
    }
}