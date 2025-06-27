using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MultiSaaS.Models;
using MultiSaaS.ViewModels;

namespace MultiSaaS.Controllers
{
    public class SubUserController : Controller
    {
        private readonly ILogger<SubUserController> _logger;

        public SubUserController(ILogger<SubUserController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the sub-user dashboard.
        /// </summary>
        public IActionResult Dashboard()
        {
            // This is a placeholder for the actual implementation
            // In a real application, you would retrieve the sub-user data from a repository
            var subUser = new SubUser
            {
                Id = 1,
                UserId = 1,
                Username = "subuser",
                Email = "subuser@example.com",
                FirstName = "Sample",
                LastName = "SubUser",
                Status = "Y",
                CreatedAt = DateTime.Now.AddMonths(-1),
                UpdatedAt = DateTime.Now
            };

            // Create a parent user for reference
            var parentUser = new User
            {
                Id = 1,
                Username = "user",
                Email = "user@example.com",
                FirstName = "Sample",
                LastName = "User",
                Phone = "123-456-7890"
            };

            // Set the navigation property
            subUser.User = parentUser;

            // Create the view model
            var viewModel = new SubUserViewModel
            {
                Id = subUser.Id,
                UserId = subUser.UserId,
                User = subUser.User,
                Username = subUser.Username,
                Email = subUser.Email,
                FirstName = subUser.FirstName,
                LastName = subUser.LastName,
                Phone = subUser.Phone,
                Role = subUser.Role,
                Status = subUser.Status,
                CreatedAt = subUser.CreatedAt,
                UpdatedAt = subUser.UpdatedAt
            };

            return View(viewModel);
        }
    }
}