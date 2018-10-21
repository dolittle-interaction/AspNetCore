/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Tenancy.Configuration;
using Microsoft.AspNetCore.Http;

namespace Dolittle.Tenancy.Strategies
{
    /// <summary>
    /// Represents a strategy than can resolve a <see cref="TenantId"/>
    /// </summary>
    public interface ICanResolveATenant : IRepresentATenantStrategy
    {
        /// <summary>
        /// Resolves a <see cref="TenantId"/>
        /// </summary>
        TenantId ResolveTenant(Func<Type, object> getConfigurationCallback, HttpRequest httpRequest);
    }
}