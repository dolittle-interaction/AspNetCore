// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.AspNetCore.Configuration
{
    /// <summary>
    /// Represents the configuration for the ExecutionContextSetup middleware.
    /// </summary>
    public class ExecutionContextSetupOptions
    {
        /// <summary>
        /// Gets or sets the name of the HTTP Header that contains the TenantId.
        /// </summary>
        public string TenantIdHeaderName { get; set; } = "Tenant-ID";

        /// <summary>
        /// Gets or sets a value indicating whether the ExecutionContextSetup middleware should skip authentication.
        /// </summary>
        public bool SkipAuthentication { get; set; } = false;
    }
}