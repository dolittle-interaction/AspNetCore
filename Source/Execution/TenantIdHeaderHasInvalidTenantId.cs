/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Dolittle.AspNetCore.Execution
{
    /// <summary>
    /// The exception that gets thrown when there is an invalid Tenant ID in the Tenant ID header on the HTTP request
    /// </summary>
    public class TenantIdHeaderHasInvalidTenantId : Exception
    {
        /// <summary>
        /// Instanciates a <see cref="TenantIdHeaderHasInvalidTenantId"/>
        /// </summary>
        /// <param name="header">The name of the HTTP header</param>
        public TenantIdHeaderHasInvalidTenantId(string header)
            : base($"The Tenant ID header '{header}' contains an invalid Tenant ID")
        {}
    }
}
