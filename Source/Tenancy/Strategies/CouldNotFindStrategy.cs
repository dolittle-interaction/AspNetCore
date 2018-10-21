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
    /// The exception that gets thrown when there is no instance of <see cref="ICanResolveATenant"/> for a <see cref="TenantStrategy"/>
    /// </summary>
    public class CouldNotFindStrategy : Exception
    {
        /// <summary>
        /// Instantiates an instance of <see cref="CouldNotFindStrategy"/>
        /// </summary>
        /// <param name="tenantStrategy"></param>
        public CouldNotFindStrategy(TenantStrategy tenantStrategy) 
            : base($"Could not find a strategy to resolve tenants for the '{tenantStrategy.Value}' strategy")
        {
        }
    }
}