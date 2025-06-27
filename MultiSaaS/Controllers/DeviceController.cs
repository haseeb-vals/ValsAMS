using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MultiSaaS.Models;
using MultiSaaS.ViewModels;

namespace MultiSaaS.Controllers
{
    public class DeviceController : Controller
    {
        // GET: Device/Create
        public IActionResult Create(int distributorId)
        {
            // In a real application, you would fetch the distributor from the database
            // For now, we'll create a mock distributor
            var distributor = new Distributor
            {
                Id = distributorId,
                Name = "Sample Distributor",
                Code = "DIST-001",
                Email = "distributor@example.com",
                Phone = "123-456-7890",
                Website = "https://distributor.example.com",
                Status = "Active",
                SlaNumber = "SLA-2023-001",
                BillingFlag = "M",
                DueDays = 10,
                OverduePercentage = 5.0m,
                MaxSubDistributionAllowed = 5,
                MaxTestingDeviceAllowed = 10,
                MaxDeviceAllowed = 100
            };

            // Get sub-distributors for dropdown
            var subDistributors = new List<SubDistributor>
            {
                new SubDistributor { Id = 1, Name = "Sub Distributor 1", DistributorId = distributorId, Status = "Active" },
                new SubDistributor { Id = 2, Name = "Sub Distributor 2", DistributorId = distributorId, Status = "Active" }
            };

            // Create a new device with default values
            var device = new Device
            {
                DistributorId = distributorId,
                Status = "Active",
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };

            // Create view model
            var viewModel = new DeviceViewModel
            {
                Device = device,
                Distributor = distributor,
                SubDistributors = subDistributors,
                DeviceTypes = new List<string> { "GPS", "Tracker", "Sensor", "Camera", "Other" },
                IsTestingDevice = false
            };

            return View(viewModel);
        }

        // POST: Device/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DeviceViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // In a real application, you would save the device to the database
                // For now, we'll just redirect to the distributor dashboard
                
                // Set creation metadata
                viewModel.Device.CreatedAt = DateTime.Now;
                viewModel.Device.CreatedBy = "System";
                viewModel.Device.IsTestingDevice = viewModel.IsTestingDevice;
                
                // Redirect to the distributor dashboard
                return RedirectToAction("Dashboard", "Distributor", new { id = viewModel.Device.DistributorId });
            }

            // If we got this far, something failed, redisplay form
            // In a real application, you would reload the distributor and sub-distributors
            return View(viewModel);
        }

        // GET: Device/Edit/5
        public IActionResult Edit(int id)
        {
            // In a real application, you would fetch the device from the database
            // For now, we'll create a mock device
            var device = new Device
            {
                Id = id,
                DistributorId = 1,
                Name = "Sample Device",
                Code = "DEV-001",
                Type = "GPS",
                Model = "Model X",
                IMEI = "123456789012345",
                IsTestingDevice = false,
                Status = "Active",
                CreatedAt = DateTime.Now.AddDays(-30),
                CreatedBy = "System",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "System"
            };

            // Mock distributor
            var distributor = new Distributor
            {
                Id = device.DistributorId,
                Name = "Sample Distributor",
                Code = "DIST-001",
                Status = "Active",
                MaxSubDistributionAllowed = 5,
                MaxTestingDeviceAllowed = 10,
                MaxDeviceAllowed = 100
            };

            // Get sub-distributors for dropdown
            var subDistributors = new List<SubDistributor>
            {
                new SubDistributor { Id = 1, Name = "Sub Distributor 1", DistributorId = device.DistributorId, Status = "Active" },
                new SubDistributor { Id = 2, Name = "Sub Distributor 2", DistributorId = device.DistributorId, Status = "Active" }
            };

            // Create view model
            var viewModel = new DeviceViewModel
            {
                Device = device,
                Distributor = distributor,
                SubDistributors = subDistributors,
                DeviceTypes = new List<string> { "GPS", "Tracker", "Sensor", "Camera", "Other" },
                IsTestingDevice = device.IsTestingDevice
            };

            return View(viewModel);
        }

        // POST: Device/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DeviceViewModel viewModel)
        {
            if (id != viewModel.Device.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // In a real application, you would update the device in the database
                // For now, we'll just redirect to the distributor dashboard
                
                // Set update metadata
                viewModel.Device.UpdatedAt = DateTime.Now;
                viewModel.Device.UpdatedBy = "System";
                viewModel.Device.IsTestingDevice = viewModel.IsTestingDevice;
                
                // Redirect to the distributor dashboard
                return RedirectToAction("Dashboard", "Distributor", new { id = viewModel.Device.DistributorId });
            }

            // If we got this far, something failed, redisplay form
            // In a real application, you would reload the distributor and sub-distributors
            return View(viewModel);
        }

        // GET: Device/Delete/5
        public IActionResult Delete(int id)
        {
            // In a real application, you would fetch the device from the database
            // For now, we'll create a mock device
            var device = new Device
            {
                Id = id,
                DistributorId = 1,
                Name = "Sample Device",
                Code = "DEV-001",
                Type = "GPS",
                Model = "Model X",
                IMEI = "123456789012345",
                IsTestingDevice = false,
                Status = "Active",
                CreatedAt = DateTime.Now.AddDays(-30),
                CreatedBy = "System",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "System"
            };

            return View(device);
        }

        // POST: Device/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // In a real application, you would delete the device from the database
            // For now, we'll just redirect to the distributor dashboard
            
            // Mock device to get the distributor ID
            var device = new Device
            {
                Id = id,
                DistributorId = 1
            };
            
            // Redirect to the distributor dashboard
            return RedirectToAction("Dashboard", "Distributor", new { id = device.DistributorId });
        }
    }
}