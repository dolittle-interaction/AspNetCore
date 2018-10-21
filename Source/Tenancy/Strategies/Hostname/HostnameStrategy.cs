/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Lifecycle;
using Dolittle.Tenancy.Configuration;
using Microsoft.AspNetCore.Http;

namespace Dolittle.Tenancy.Strategies.Hostname
{
    /// <inheritdoc/>
    [Singleton]
    public class HostnameStrategy : ICanResolveATenant
    {
        /// <inheritdoc/>
        public TenantStrategy Strategy => "hostname";
        /// <inheritdoc/>
        public Type StrategyConfigurationType => typeof(HostnameStrategyResource);
        
        /// <inheritdoc/>
        public TenantId ResolveTenant(Func<Type, object> getConfigurationCallback, HttpRequest httpRequest)
        {
            var configuration = (HostnameStrategyResource) getConfigurationCallback(StrategyConfigurationType);
            
            switch (configuration.Configuration.Segments)
            {
                case Segments.All:
                    return ResolveTenantIdForAllSegments(httpRequest.Host);
                case Segments.First:
                    return ResolveTenantIdForFirstSegment(httpRequest.Host);
                default:
                    return TenantId.Unknown;
            }
        }

        TenantId ResolveTenantIdForAllSegments(HostString host)
        {
            return TenantId.System;
        }
        TenantId ResolveTenantIdForFirstSegment(HostString host)
        {
            return TenantId.System;
        }
    }
}