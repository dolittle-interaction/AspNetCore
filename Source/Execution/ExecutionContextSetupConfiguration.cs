/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Dolittle.AspNetCore.Execution
{
    /// <summary>
    /// Represents the configuration for the <see cref="ExecutionContextSetup"/> middleware
    /// </summary>
    public class ExecutionContextSetupConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the HTTP Header that contains the <see cref="Dolittle.Tenancy.TenantId"/> 
        /// </summary>
        public string TenantIdHeaderName {get; set;} = "Tenant-ID";
    }
}