/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Runtime.Serialization;

namespace Dolittle.Tenancy.Strategies.Hostname
{
    /// <summary>
    /// The exception that gets thrown when resolving the tenant with the hostname strategy and the tenant segment of the host did not match any of the pre-configured mappings 
    /// </summary>
    public class CouldNotMapTenantSegmentToTenantId : Exception
    {
        /// <summary>
        /// Instantiates an instance of <see cref="CouldNotMapTenantSegmentToTenantId"/>
        /// </summary>
        /// <param name="tenantSegment"></param>
        /// <returns></returns>
        public CouldNotMapTenantSegmentToTenantId(string tenantSegment) 
            : base($"Could not map the tenant segment '{tenantSegment}' with any of the pre-configured mappings")
        {}
    }
}