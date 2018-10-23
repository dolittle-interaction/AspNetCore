/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Dolittle.Tenancy.Strategies.Hostname
{
    /// <summary>
    /// The exception that gets thrown when 
    /// </summary>
    public class CannotResolveTenantFromHostname : Exception
    {
        /// <summary>
        /// Instantiates an instance of <see cref="CannotResolveTenantFromHostname"/>
        /// </summary>
        /// <param name="host"></param>
        public CannotResolveTenantFromHostname(string host)
            : base($"Could not resolve the tenant from host: '{host}'")
        {}
    }
}