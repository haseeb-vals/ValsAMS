using System;

namespace MultiSaaS.Tenant
{
    /// <summary>
    /// Implementation of ITenantProvider that manages tenant context.
    /// </summary>
    public class TenantProvider : ITenantProvider
    {
        private int _currentTenantId;
        
        /// <summary>
        /// Initializes a new instance of the TenantProvider class with a default tenant ID.
        /// </summary>
        public TenantProvider()
        {
            // Default tenant ID for development purposes
            _currentTenantId = 1;
        }
        
        /// <inheritdoc/>
        public int GetTenantId()
        {
            return _currentTenantId;
        }
        
        /// <inheritdoc/>
        public void SetTenantId(int tenantId)
        {
            if (tenantId <= 0)
            {
                throw new ArgumentException("Tenant ID must be a positive integer.", nameof(tenantId));
            }
            _currentTenantId = tenantId;
        }
    }
}