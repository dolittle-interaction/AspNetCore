/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Dolittle.AspNetCore.Configuration
{
    /// <summary>
    /// Represents the configuration for Dolittle services and middlewares
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets or sets the configuration of the ExecutionContextSetup middleware <see cref="Dolittle.AspNetCore.Configuration.ExecutionContextSetupOptions"/> 
        /// </summary>
        public ExecutionContextSetupOptions ExecutionContextSetup { get; set; }
    }
}
