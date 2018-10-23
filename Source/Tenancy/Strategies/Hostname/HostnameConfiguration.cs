/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Tenancy.Configuration;

namespace Dolittle.Tenancy.Strategies.Hostname
{
    /// <summary>
    /// Represents the configuration for the Hostname <see cref="TenantStrategy">tenant strategy</see>
    /// </summary>
    public class HostnameConfiguration
    {
        /// <summary>
        /// Gets the enum representing how
        /// </summary>
        /// <value></value>
        public Segments Segments {get; set; }
    }
}