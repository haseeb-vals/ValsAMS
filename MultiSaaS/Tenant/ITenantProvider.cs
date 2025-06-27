using System;

namespace MultiSaaS.Tenant
{
    /// <summary>
    /// Interface for tenant resolution and context management.
    /// Provides methods to identify and retrieve tenant information during request lifecycle.
    /// </summary>
    public interface ITenantProvider
    {
        /// <summary>
        /// Gets the current tenant ID from the context.
        /// </summary>
        /// <returns>The tenant ID as an integer.</returns>
        int GetTenantId();
        
        /// <summary>
        /// Sets the current tenant ID in the context.
        /// </summary>
        /// <param name="tenantId">The tenant ID to set.</param>
        void SetTenantId(int tenantId);
    }
}