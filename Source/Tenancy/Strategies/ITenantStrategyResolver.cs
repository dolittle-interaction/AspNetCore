/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Tenancy.Configuration;
using Microsoft.AspNetCore.Http;

namespace Dolittle.Tenancy.Strategies
{
    /// <summary>
    /// Represents a system that resolves a <see cref="TenantId"/> by finding a strategy that can resolve it 
    /// </summary>
    public interface ITenantResolverForStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantStrategy"></param>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        TenantId Resolve(TenantStrategy tenantStrategy, HttpRequest httpRequest);
    }
}