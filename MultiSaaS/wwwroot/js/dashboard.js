/**
 * Dashboard functionality for MultiSaaS
 * Handles sidebar, page navigation, and responsive behavior
 */

function initDashboard() {
    // Initialize sidebar functionality
    setupSidebar();
    
    // Initialize page navigation
    setupPageNavigation();
    
    // Initialize theme toggle
    setupThemeToggle();
    
    // Initialize form handling
    setupFormHandling();
    
    // Initialize responsive behavior
    setupResponsiveBehavior();
    
    // Initialize logout functionality
    setupLogout();
}

function setupSidebar() {
    // Mobile menu toggle
    const sidebarToggle = document.getElementById('sidebar-toggle');
    const sidebarClose = document.getElementById('sidebar-close');
    const sidebar = document.querySelector('.sidebar');
    
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', function() {
            sidebar.classList.add('expanded');
            sidebar.classList.remove('collapsed');
        });
    }
    
    if (sidebarClose) {
        sidebarClose.addEventListener('click', function() {
            sidebar.classList.remove('expanded');
            sidebar.classList.add('collapsed');
        });
    }
    
    // Check if we're on a mobile device and set appropriate classes
    function checkMobileView() {
        if (window.innerWidth < 768) {
            sidebar.classList.add('collapsed');
            sidebar.classList.remove('expanded');
        } else {
            sidebar.classList.add('expanded');
            sidebar.classList.remove('collapsed');
        }
    }
    
    // Run on page load
    checkMobileView();
    
    // Run on window resize
    window.addEventListener('resize', checkMobileView);
}

function setupPageNavigation() {
    const menuItems = document.querySelectorAll('.sidebar-menu li');
    const contentPages = document.querySelectorAll('.content-page');
    const pageTitle = document.getElementById('page-title');
    
    menuItems.forEach(item => {
        item.addEventListener('click', function() {
            // Get the page identifier
            const pageId = this.getAttribute('data-page');
            if (!pageId) return;
            
            // Update active state in sidebar
            menuItems.forEach(i => i.classList.remove('active'));
            this.classList.add('active');
            
            // Show the corresponding page content
            contentPages.forEach(page => {
                page.classList.remove('active');
                if (page.id === pageId + '-page') {
                    page.classList.add('active');
                }
            });
            
            // Update page title
            if (pageTitle) {
                const menuText = this.querySelector('span');
                if (menuText) {
                    pageTitle.textContent = menuText.textContent;
                }
            }
            
            // On mobile, close the sidebar after navigation
            if (window.innerWidth < 768) {
                const sidebar = document.querySelector('.sidebar');
                sidebar.classList.remove('expanded');
                sidebar.classList.add('collapsed');
            }
        });
    });
}

function setupThemeToggle() {
    const themeSwitch = document.getElementById('theme-switch');
    
    if (themeSwitch) {
        // Check if theme preference exists in localStorage
        const darkTheme = localStorage.getItem('darkTheme') === 'true';
        
        // Set initial state
        document.body.classList.toggle('dark-theme', darkTheme);
        themeSwitch.checked = darkTheme;
        
        // Handle theme toggle
        themeSwitch.addEventListener('change', function() {
            const isDarkTheme = this.checked;
            document.body.classList.toggle('dark-theme', isDarkTheme);
            localStorage.setItem('darkTheme', isDarkTheme);
        });
    }
}

function setupFormHandling() {
    // Handle AJAX form submissions
    const ajaxForms = document.querySelectorAll('.ajax-form');
    
    ajaxForms.forEach(form => {
        form.addEventListener('submit', function(e) {
            e.preventDefault();
            
            // Get form data
            const formData = new FormData(this);
            
            // Get submit button
            const submitBtn = this.querySelector('[type="submit"]');
            const originalBtnText = submitBtn ? submitBtn.innerHTML : 'Save';
            
            // Show loading state
            if (submitBtn) {
                submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Saving...';
                submitBtn.disabled = true;
            }
            
            // Send AJAX request
            fetch(this.action, {
                method: this.method || 'POST',
                body: formData,
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            })
            .then(response => response.json())
            .then(data => {
                // Show success message
                showAlert('success', data.message || 'Changes saved successfully!');
                
                // If there's a redirect URL, redirect after a delay
                if (data.redirectUrl) {
                    setTimeout(() => {
                        window.location.href = data.redirectUrl;
                    }, 1500);
                }
            })
            .catch(error => {
                // Show error message
                showAlert('danger', 'An error occurred while saving changes.');
                console.error('Form submission error:', error);
            })
            .finally(() => {
                // Restore button state
                if (submitBtn) {
                    submitBtn.innerHTML = originalBtnText;
                    submitBtn.disabled = false;
                }
            });
        });
    });
    
    // Form validation
    const forms = document.querySelectorAll('.needs-validation');
    
    forms.forEach(form => {
        form.addEventListener('submit', function(event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            
            form.classList.add('was-validated');
        }, false);
    });
}

function setupResponsiveBehavior() {
    // Make cards responsive
    const adjustCardHeights = () => {
        const cardRows = document.querySelectorAll('.row');
        
        cardRows.forEach(row => {
            // Reset heights
            const cards = row.querySelectorAll('.card');
            cards.forEach(card => card.style.height = 'auto');
            
            // Only equalize heights on medium and larger screens
            if (window.innerWidth >= 768) {
                // Find the tallest card
                let maxHeight = 0;
                cards.forEach(card => {
                    maxHeight = Math.max(maxHeight, card.offsetHeight);
                });
                
                // Set all cards to the same height
                cards.forEach(card => card.style.height = maxHeight + 'px');
            }
        });
    };
    
    // Run on page load and window resize
    adjustCardHeights();
    window.addEventListener('resize', adjustCardHeights);
    
    // Adjust form layouts on small screens
    const adjustFormLayouts = () => {
        const forms = document.querySelectorAll('form');
        
        forms.forEach(form => {
            const formRows = form.querySelectorAll('.row');
            
            formRows.forEach(row => {
                const cols = row.querySelectorAll('[class*="col-"]');
                
                if (window.innerWidth < 576) {
                    cols.forEach(col => {
                        // Add mb-3 class for spacing on mobile
                        if (!col.classList.contains('mb-3')) {
                            col.classList.add('mb-3');
                        }
                    });
                } else {
                    cols.forEach(col => {
                        // Remove mb-3 class on larger screens if it's the last column
                        if (col === cols[cols.length - 1]) {
                            col.classList.remove('mb-3');
                        }
                    });
                }
            });
        });
    };
    
    // Run on page load and window resize
    adjustFormLayouts();
    window.addEventListener('resize', adjustFormLayouts);
}

function setupLogout() {
    // Handle logout functionality
    const logoutLinks = document.querySelectorAll('a[onclick="logout()"]');
    
    logoutLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            logout();
        });
    });
}

function logout() {
    // Clear any user session data from localStorage
    localStorage.removeItem('darkTheme');
    
    // Redirect to login page
    window.location.href = '/';
}

function showAlert(type, message) {
    // Create alert container if it doesn't exist
    let alertContainer = document.querySelector('.alert-container');
    
    if (!alertContainer) {
        alertContainer = document.createElement('div');
        alertContainer.className = 'alert-container position-fixed top-0 end-0 p-3';
        alertContainer.style.zIndex = '1050';
        document.body.appendChild(alertContainer);
    }
    
    // Create alert element
    const alert = document.createElement('div');
    alert.className = `alert alert-${type} alert-dismissible fade show`;
    alert.role = 'alert';
    alert.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;
    
    // Add alert to container
    alertContainer.appendChild(alert);
    
    // Auto-dismiss after 5 seconds
    setTimeout(() => {
        alert.classList.remove('show');
        setTimeout(() => {
            alert.remove();
        }, 150);
    }, 5000);
}

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    // Check if we're on a dashboard page
    if (document.querySelector('.dashboard-container')) {
        initDashboard();
    }
});