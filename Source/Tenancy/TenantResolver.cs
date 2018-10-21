/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Tenancy.Strategies;
using Dolittle.Tenancy.Configuration;
using Microsoft.AspNetCore.Http;

namespace Dolittle.Tenancy
{

    /// <inheritdoc/>
    public class TenantResolver : ITenantResolver
    {
        readonly ITenantMapManager _tenantMapManager;
        readonly ITenantResolverForStrategy _tenantResolverForStrategy;

        /// <inheritdoc/>    
        public TenantResolver(ITenantMapManager tenantMapManager, ITenantResolverForStrategy tenantResolverForStrategy)
        {
            _tenantMapManager = tenantMapManager;
            _tenantResolverForStrategy = tenantResolverForStrategy;
            
        }
        /// <inheritdoc/>
        public TenantStrategy CurrentTenantResolvingStrategy => _tenantMapManager.Strategy;

        /// <inheritdoc/>
        public TenantId Resolve(HttpRequest httpRequest)
        {
            var currentStrategy = CurrentTenantResolvingStrategy;

            return _tenantResolverForStrategy.Resolve(currentStrategy, httpRequest);
            
        }
    }
}