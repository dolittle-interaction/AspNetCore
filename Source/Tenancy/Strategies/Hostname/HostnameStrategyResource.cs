/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Dolittle.Tenancy.Strategies.Hostname
{
    /// <summary>
    /// Represents the actual configuration and mapping for the strategy
    /// </summary>
    public class HostnameStrategyResource
    {
        /// <summary>
        /// Gets whether or not the tenant strategy system should fallback to the Development <see cref="TenantId"/> if tenant could not resolved 
        /// </summary>
        /// <value></value>
        public bool FallbackToDeveloperTenant {get; set;} 
        /// <summary>
        /// Gets the Configuration
        /// </summary>
        public HostnameConfiguration Configuration {get; set;}
        
        /// <summary>
        /// Gets the map that represents the mapping of the configuration
        /// </summary>
        public IDictionary<TenantSegment, TenantId> Map {get; set; }
    }
}