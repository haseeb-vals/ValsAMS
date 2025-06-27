using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MultiSaaS.Models;
using MultiSaaS.ViewModels;

namespace MultiSaaS.Controllers
{
    public class DistributorController : Controller
    {
        // GET: Distributor/Dashboard
        public IActionResult Dashboard()
        {
            // This is a mock implementation for demonstration purposes
            // In a real application, this would retrieve data from a database
            var viewModel = new DistributorViewModel
            {
                Distributor = new Distributor
                {
                    Id = 1,
                    Name = "Sample Distributor",
                    Code = "DIST001",
                    Description = "This is a sample distributor for demonstration purposes.",
                    Email = "contact@sampledistributor.com",
                    Phone = "(555) 123-4567",
                    Address = "123 Main Street",
                    City = "Anytown",
                    State = "Anystate",
                    Country = "United States",
                    PostalCode = "12345",
                    Website = "https://www.sampledistributor.com",
                    Status = "Active",
                    SlaNumber = "SLA-2023-001",
                    BillingFlag = "M",
                    DueDays = 15,
                    OverduePercentage = 5.25m,
                    MaxSubDistributionAllowed = 5,
                    MaxTestingDeviceAllowed = 10,
                    MaxDeviceAllowed = 100
                },
                Channels = new List<DistributorChannel>
                {
                    new DistributorChannel
                    {
                        Id = 1,
                        DistributorId = 1,
                        Name = "Retail",
                        Code = "CHAN001",
                        Description = "Retail channel for direct consumer sales.",
                        Type = "Retail",
                        Status = "Active"
                    },
                    new DistributorChannel
                    {
                        Id = 2,
                        DistributorId = 1,
                        Name = "Wholesale",
                        Code = "CHAN002",
                        Description = "Wholesale channel for business-to-business sales.",
                        Type = "Wholesale",
                        Status = "Active"
                    }
                },
                PaymentTerms = new List<DistributorPaymentTerm>
                {
                    new DistributorPaymentTerm
                    {
                        Id = 1,
                        DistributorId = 1,
                        Name = "Net 30",
                        Code = "TERM001",
                        Description = "Payment due within 30 days.",
                        DurationDays = 30,
                        Status = "Active"
                    },
                    new DistributorPaymentTerm
                    {
                        Id = 2,
                        DistributorId = 1,
                        Name = "Net 60",
                        Code = "TERM002",
                        Description = "Payment due within 60 days.",
                        DurationDays = 60,
                        Status = "Active"
                    }
                },
                SubDistributors = new List<SubDistributor>
                {
                    new SubDistributor
                    {
                        Id = 1,
                        DistributorId = 1,
                        Name = "Sample Sub-Distributor 1",
                        Code = "SUBDIST001",
                        Email = "contact@samplesubdistributor1.com",
                        Phone = "(555) 987-6543",
                        Status = "Active"
                    },
                    new SubDistributor
                    {
                        Id = 2,
                        DistributorId = 1,
                        Name = "Sample Sub-Distributor 2",
                        Code = "SUBDIST002",
                        Email = "contact@samplesubdistributor2.com",
                        Phone = "(555) 456-7890",
                        Status = "Active"
                    }
                },
                Devices = new List<Device>
                {
                    new Device
                    {
                        Id = 1,
                        DistributorId = 1,
                        Name = "GPS Tracker 1",
                        Code = "DEV001",
                        Type = "Hardwired GPS",
                        Model = "Teltonika FMB920",
                        IMEI = "123456789012345",
                        IsTestingDevice = false,
                        Status = "Active"
                    },
                    new Device
                    {
                        Id = 2,
                        DistributorId = 1,
                        Name = "GPS Tracker 2",
                        Code = "DEV002",
                        Type = "Battery Tracker",
                        Model = "Tracki 2023",
                        IMEI = "987654321098765",
                        IsTestingDevice = false,
                        Status = "Active"
                    },
                    new Device
                    {
                        Id = 3,
                        DistributorId = 1,
                        Name = "Test Device 1",
                        Code = "TEST001",
                        Type = "OBD-II Tracker",
                        Model = "Teltonika FMB630",
                        IMEI = "555666777888999",
                        IsTestingDevice = true,
                        Status = "Active"
                    }
                }
            };
            
            return View(viewModel);
        }
        
        // GET: Distributor/Create
        public IActionResult Create()
        {
            return View();
        }
        
        // POST: Distributor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Distributor distributor)
        {
            if (ModelState.IsValid)
            {
                // Save the distributor to the database
                // In a real application, this would use a service or repository
                
                return RedirectToAction(nameof(Dashboard));
            }
            return View(distributor);
        }
        
        // GET: Distributor/Edit/5
        public IActionResult Edit(int id)
        {
            // In a real application, this would retrieve the distributor from a database
            var distributor = new Distributor
            {
                Id = id,
                Name = "Sample Distributor",
                Code = "DIST001",
                Description = "This is a sample distributor for demonstration purposes.",
                Email = "contact@sampledistributor.com",
                Phone = "(555) 123-4567",
                Address = "123 Main Street",
                City = "Anytown",
                State = "Anystate",
                Country = "United States",
                PostalCode = "12345",
                Website = "https://www.sampledistributor.com",
                Status = "Active",
                SlaNumber = "SLA-2023-001",
                BillingFlag = "M",
                DueDays = 15,
                OverduePercentage = 5.25m,
                MaxSubDistributionAllowed = 5,
                MaxTestingDeviceAllowed = 10,
                MaxDeviceAllowed = 100
            };
            
            return View(distributor);
        }
        
        // POST: Distributor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Distributor distributor)
        {
            if (id != distributor.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                // Update the distributor in the database
                // In a real application, this would use a service or repository
                
                return RedirectToAction(nameof(Dashboard));
            }
            return View(distributor);
        }
        
        // GET: Distributor/Delete/5
        public IActionResult Delete(int id)
        {
            // In a real application, this would retrieve the distributor from a database
            var distributor = new Distributor
            {
                Id = id,
                Name = "Sample Distributor",
                Code = "DIST001"
            };
            
            return View(distributor);
        }
        
        // POST: Distributor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete the distributor from the database
            // In a real application, this would use a service or repository
            
            return RedirectToAction(nameof(Dashboard));
        }
    }
}