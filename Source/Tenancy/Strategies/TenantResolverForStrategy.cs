/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;
using Dolittle.Lifecycle;
using Dolittle.Tenancy.Configuration;
using Dolittle.Types;
using Microsoft.AspNetCore.Http;

namespace Dolittle.Tenancy.Strategies
{
    /// <inheritdoc/>
    [Singleton]
    public class TenantResolverForStrategy : ITenantResolverForStrategy
    {
        readonly IEnumerable<ICanResolveATenant> _tenantResolvers;
        readonly ITenantMapManager _tenantMapManager;
        /// <inheritdoc/>
        public TenantResolverForStrategy(IInstancesOf<ICanResolveATenant> instancesOfTenantResolver, ITenantMapManager tenantMapManager)
        {
            _tenantResolvers = instancesOfTenantResolver;
            _tenantMapManager = tenantMapManager;
        }
        /// <inheritdoc/>
        public TenantId Resolve(TenantStrategy tenantStrategy, HttpRequest httpRequest)
        {
            var strategiesThatCanResolveTenant = _tenantResolvers.Where(_ => _.Strategy.Value.Equals(tenantStrategy.Value)).ToArray();
            var length = strategiesThatCanResolveTenant.Length;
            if (length == 0) throw new CouldNotFindStrategy(tenantStrategy);
            if (length > 1) throw new MultipleStrategiesFound(tenantStrategy);

            return strategiesThatCanResolveTenant[0].ResolveTenant((strategyType) => _tenantMapManager.InstanceOfStrategy(strategyType),httpRequest);
        }
    }
}