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
        const char _hostnameSegmentSeparator = '.';
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
                    return ResolveTenantIdForAllSegments(httpRequest.Host, configuration);
                case Segments.First:
                    return ResolveTenantIdForFirstSegment(httpRequest.Host, configuration);
                default:
                    return TenantId.Unknown;
            }
        }

        TenantId ResolveTenantIdForAllSegments(HostString host, HostnameStrategyResource configuration)
        {
            var tenantSegment = host.Host;
            if (! configuration.Map.ContainsKey(tenantSegment)) throw new CouldNotMapTenantSegmentToTenantId(tenantSegment);
            return configuration.Map[tenantSegment];
        }
        TenantId ResolveTenantIdForFirstSegment(HostString host, HostnameStrategyResource configuration)
        {
            var segments = host.Host.Split(_hostnameSegmentSeparator);
            if (segments.Length < 2) throw new CannotResolveTenantFromHostname(host.Host);
            var tenantSegment = segments[0];
            if (! configuration.Map.ContainsKey(tenantSegment)) throw new CouldNotMapTenantSegmentToTenantId(tenantSegment);
            return configuration.Map[tenantSegment];
        }
    }
}