/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Dolittle.AspNetCore.Execution
{

    /// <summary>
    /// The exception that gets thrown when there is a Tenant ID header on the http request with multiple values
    /// </summary>
    [Serializable]
    public class TenantIdHeaderHasMultipleValues : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TenantIdHeaderHasMultipleValues"/>
        /// </summary>
        /// <param name="header">The name of the claim</param>
        public TenantIdHeaderHasMultipleValues(string header)
            : base($"There are multiple values for Tenant ID header '{header}'")
        {}
    }
}
