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
        /// The Default <see cref="ExecutionContextSetupConfigurationDelegate"/>
        /// </summary>
        public static ExecutionContextSetupConfigurationDelegate DefaultDelegate = _ => ExecutionContextSetupConfiguration.Default;

        /// <summary>
        /// The Default <see cref="ExecutionContextSetupConfiguration"/>
        /// </summary>
        public static ExecutionContextSetupConfiguration Default => new ExecutionContextSetupConfiguration{TenantIdHeaderKey = "Tenant-ID"}; 

        /// <summary>
        /// Gets or sets the HTTP Header key for the <see cref="Dolittle.Tenancy.TenantId"/> 
        /// </summary>
        public string TenantIdHeaderKey {get; set;}
    }
}