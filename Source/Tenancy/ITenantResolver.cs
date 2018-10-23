/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Tenancy.Configuration;
using Microsoft.AspNetCore.Http;

namespace Dolittle.Tenancy
{
    /// <summary>
    /// Represents a system that resolves the <see cref="TenantId"/>
    /// </summary>
    public interface ITenantResolver
    {
        /// <summary>
        /// Resolves the <see cref="TenantId"/>
        /// </summary>
        /// <returns>The resolved <see cref="TenantId"/></returns>
        TenantId Resolve(HttpRequest httpRequest);
    }
}