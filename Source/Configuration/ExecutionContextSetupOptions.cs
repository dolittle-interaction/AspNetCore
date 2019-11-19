/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Dolittle.AspNetCore.Configuration
{
    /// <summary>
    /// Represents the configuration for the ExecutionContextSetup middleware
    /// </summary>
    public class ExecutionContextSetupOptions
    {
        /// <summary>
        /// Gets or sets the name of the HTTP Header that contains the TenantId 
        /// </summary>
        public string TenantIdHeaderName { get; set; } = "Tenant-ID";
        /// <summary>
        /// Gets or sets whether the ExecutionContextSetup middleware should skip authentication
        /// </summary>
        public bool SkipAuthentication { get; set; } = false;
    }
}