// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Dashboard Manager
const DashboardManager = {
    init: function() {
        this.setupSidebar();
        this.setupThemeToggle();
        this.setupLogout();
        this.setupPageNavigation();
        this.setupResponsiveElements();
    },
    
    setupSidebar: function() {
        const sidebar = document.querySelector('.sidebar');
        const content = document.querySelector('.content');
        const toggleBtn = document.querySelector('.sidebar-toggle');
        const pinBtn = document.querySelector('.sidebar-pin');
        const mobileMenuToggle = document.querySelector('.mobile-menu-toggle');
        const sidebarCloseBtn = document.querySelector('.sidebar-close');
        
        if (!sidebar || !content) return;
        
        // Load sidebar state from localStorage
        const sidebarState = localStorage.getItem('sidebarState');
        if (sidebarState === 'collapsed') {
            sidebar.classList.add('collapsed');
            content.classList.add('expanded');
        }
        
        const isPinned = localStorage.getItem('sidebarPinned') === 'true';
        if (isPinned) {
            sidebar.classList.add('pinned');
            if (pinBtn) pinBtn.classList.add('active');
        }
        
        // Toggle sidebar
        if (toggleBtn) {
            toggleBtn.addEventListener('click', function() {
                sidebar.classList.toggle('collapsed');
                content.classList.toggle('expanded');
                
                // Save state to localStorage
                if (sidebar.classList.contains('collapsed')) {
                    localStorage.setItem('sidebarState', 'collapsed');
                } else {
                    localStorage.setItem('sidebarState', 'expanded');
                }
            });
        }
        
        // Mobile menu toggle
        if (mobileMenuToggle) {
            mobileMenuToggle.addEventListener('click', function() {
                sidebar.classList.add('expanded');
                sidebar.classList.remove('collapsed');
                sidebar.style.left = '0';
            });
        }
        
        // Sidebar close button
        if (sidebarCloseBtn) {
            sidebarCloseBtn.addEventListener('click', function() {
                sidebar.style.left = '-250px';
            });
        }
        
        // Pin sidebar
        if (pinBtn) {
            pinBtn.addEventListener('click', function() {
                sidebar.classList.toggle('pinned');
                this.classList.toggle('active');
                
                // Save pinned state to localStorage
                localStorage.setItem('sidebarPinned', sidebar.classList.contains('pinned'));
            });
        }
        
        // Hover effect for collapsed sidebar
        sidebar.addEventListener('mouseenter', function() {
            if (sidebar.classList.contains('collapsed') && !sidebar.classList.contains('pinned')) {
                sidebar.classList.add('expanded');
            }
        });
        
        sidebar.addEventListener('mouseleave', function() {
            if (sidebar.classList.contains('collapsed') && !sidebar.classList.contains('pinned')) {
                sidebar.classList.remove('expanded');
            }
        });
    },
    
    setupThemeToggle: function() {
        const themeToggleBtn = document.getElementById('themeToggleBtn');
        const themeIcon = themeToggleBtn ? themeToggleBtn.querySelector('.theme-icon') : null;
        const themeText = themeToggleBtn ? themeToggleBtn.querySelector('.btn-text') : null;
        
        // Initialize theme from localStorage if available
        const darkTheme = localStorage.getItem('darkTheme') === 'true';
        if (darkTheme) {
            document.body.classList.add('dark-theme');
            if (themeIcon) {
                themeIcon.classList.remove('fa-moon');
                themeIcon.classList.add('fa-sun');
            }
            if (themeText) {
                themeText.textContent = 'Light Mode';
            }
        }
        
        // Toggle theme
        if (themeToggleBtn) {
            themeToggleBtn.addEventListener('click', function() {
                const isDarkTheme = document.body.classList.toggle('dark-theme');
                
                if (themeIcon) {
                    if (isDarkTheme) {
                        themeIcon.classList.remove('fa-moon');
                        themeIcon.classList.add('fa-sun');
                        if (themeText) themeText.textContent = 'Light Mode';
                    } else {
                        themeIcon.classList.remove('fa-sun');
                        themeIcon.classList.add('fa-moon');
                        if (themeText) themeText.textContent = 'Dark Mode';
                    }
                }
                
                // Save theme preference to localStorage
                localStorage.setItem('darkTheme', isDarkTheme);
            });
        }
    },
    
    setupLogout: function() {
        const logoutBtn = document.getElementById('logoutBtn');
        
        if (logoutBtn) {
            logoutBtn.addEventListener('click', function() {
                // Clear any user session data from localStorage
                localStorage.removeItem('sidebarState');
                
                // Redirect to login page
                window.location.href = '/';
            });
        }
    },
    
    setupPageNavigation: function() {
        const menuItems = document.querySelectorAll('.sidebar-item a');
        const contentPages = document.querySelectorAll('.content-page');
        const pageTitle = document.querySelector('.page-title h2');
        const pageButtons = document.querySelectorAll('button[data-page]');
        
        if (!contentPages.length) return;
        
        // Load active page from localStorage
        const activePage = localStorage.getItem('activePage');
        if (activePage) {
            // Hide all pages
            contentPages.forEach(page => page.classList.remove('active'));
            
            // Show active page
            const targetPage = document.getElementById(activePage);
            if (targetPage) {
                targetPage.classList.add('active');
                
                // Update menu active state
                menuItems.forEach(item => {
                    item.parentElement.classList.remove('active');
                    if (item.getAttribute('data-page') === activePage) {
                        item.parentElement.classList.add('active');
                        if (pageTitle) pageTitle.textContent = item.querySelector('.menu-text').textContent;
                    }
                });
            }
        } else {
            // Default to first page
            if (contentPages.length > 0) {
                contentPages[0].classList.add('active');
                if (menuItems.length > 0) {
                    menuItems[0].parentElement.classList.add('active');
                    if (pageTitle) pageTitle.textContent = menuItems[0].querySelector('.menu-text').textContent;
                }
            }
        }
        
        // Handle menu item clicks
        menuItems.forEach(item => {
            item.addEventListener('click', function(e) {
                e.preventDefault();
                
                const targetPageId = this.getAttribute('data-page');
                if (!targetPageId) return;
                
                // Update active menu item
                menuItems.forEach(menuItem => menuItem.parentElement.classList.remove('active'));
                this.parentElement.classList.add('active');
                
                // Update page title
                if (pageTitle) pageTitle.textContent = this.querySelector('.menu-text').textContent;
                
                // Show target page
                contentPages.forEach(page => page.classList.remove('active'));
                const targetPage = document.getElementById(targetPageId);
                if (targetPage) {
                    targetPage.classList.add('active');
                    
                    // Save active page to localStorage
                    localStorage.setItem('activePage', targetPageId);
                }
                
                // On mobile, close sidebar after navigation
                const sidebar = document.querySelector('.sidebar');
                if (window.innerWidth < 992 && sidebar) {
                    sidebar.classList.remove('expanded');
                    sidebar.style.left = '-250px';
                }
            });
        });
        
        // Handle page navigation buttons
        if (pageButtons.length) {
            pageButtons.forEach(button => {
                button.addEventListener('click', function() {
                    const targetPageId = this.getAttribute('data-page');
                    if (!targetPageId) return;
                    
                    // Show target page
                    contentPages.forEach(page => page.classList.remove('active'));
                    const targetPage = document.getElementById(targetPageId);
                    if (targetPage) {
                        targetPage.classList.add('active');
                        
                        // Update menu active state
                        menuItems.forEach(item => {
                            item.parentElement.classList.remove('active');
                            if (item.getAttribute('data-page') === targetPageId) {
                                item.parentElement.classList.add('active');
                                if (pageTitle) pageTitle.textContent = item.querySelector('.menu-text').textContent;
                            }
                        });
                        
                        // Save active page to localStorage
                        localStorage.setItem('activePage', targetPageId);
                    }
                });
            });
        }
    },
    
    setupResponsiveElements: function() {
        // Handle dropdown menus
        const dropdownToggles = document.querySelectorAll('.dropdown-toggle');
        if (dropdownToggles.length) {
            dropdownToggles.forEach(toggle => {
                toggle.addEventListener('click', function(e) {
                    e.preventDefault();
                    const dropdownMenu = this.nextElementSibling;
                    if (dropdownMenu && dropdownMenu.classList.contains('dropdown-menu')) {
                        dropdownMenu.classList.toggle('show');
                    }
                });
            });
            
            // Close dropdowns when clicking outside
            document.addEventListener('click', function(e) {
                if (!e.target.closest('.dropdown')) {
                    document.querySelectorAll('.dropdown-menu.show').forEach(menu => {
                        menu.classList.remove('show');
                    });
                }
            });
        }
        
        // Handle responsive tables
        const tables = document.querySelectorAll('table');
        if (tables.length) {
            tables.forEach(table => {
                if (!table.parentElement.classList.contains('table-responsive')) {
                    const wrapper = document.createElement('div');
                    wrapper.className = 'table-responsive';
                    table.parentNode.insertBefore(wrapper, table);
                    wrapper.appendChild(table);
                }
            });
        }
        
        // Add responsive behavior to cards
        const statCards = document.querySelectorAll('.stat-card');
        if (statCards.length) {
            const updateCardLayout = () => {
                if (window.innerWidth < 576) {
                    statCards.forEach(card => {
                        if (!card.classList.contains('flex-column')) {
                            card.classList.add('flex-column');
                            card.classList.add('align-items-center');
                            card.classList.add('text-center');
                        }
                    });
                } else {
                    statCards.forEach(card => {
                        card.classList.remove('flex-column');
                        card.classList.remove('align-items-center');
                        card.classList.remove('text-center');
                    });
                }
            };
            
            // Initial call
            updateCardLayout();
            
            // Update on resize
            window.addEventListener('resize', updateCardLayout);
        }
    }
};

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    DashboardManager.init();
});
