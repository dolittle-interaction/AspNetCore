/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Runtime.Serialization;
using Dolittle.Tenancy.Configuration;

namespace Dolittle.Tenancy.Strategies
{
    /// <summary>
    /// The exception that gets thrown when there are multiple instances of <see cref="ICanResolveATenant"/> for a <see cref="TenantStrategy"/>
    /// </summary>
    public class MultipleStrategiesFound : Exception
    {
        /// <summary>
        /// Instantiates an instance of <see cref="MultipleStrategiesFound"/>
        /// </summary>
        /// <param name="tenantStrategy"></param>
        public MultipleStrategiesFound(TenantStrategy tenantStrategy)
            : base($"Found multiple strategies for the {tenantStrategy.Value} strategy")
        {
        }
    }
}