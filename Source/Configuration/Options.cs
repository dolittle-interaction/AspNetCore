// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.AspNetCore.Configuration
{
    /// <summary>
    /// Represents the configuration for Dolittle services and middlewares.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets the configuration of the ExecutionContextSetup middleware <see cref="ExecutionContextSetupOptions"/>.
        /// </summary>
        public ExecutionContextSetupOptions ExecutionContextSetup { get; } = new ExecutionContextSetupOptions();
    }
}
