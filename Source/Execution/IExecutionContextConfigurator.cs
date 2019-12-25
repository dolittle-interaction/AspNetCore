// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Execution;
using Dolittle.Security;
using Dolittle.Tenancy;

namespace Dolittle.AspNetCore.Execution
{
    /// <summary>
    /// A system that configures the <see cref="ExecutionContext"/>.
    /// </summary>
    public interface IExecutionContextConfigurator
    {
        /// <summary>
        /// Gets the current <see cref="ExecutionContext"/> from the <see cref="IExecutionContextManager"/>.
        /// </summary>
        /// <returns>The current <see cref="ExecutionContext"/>.</returns>
        ExecutionContext CurrentExecutionContext();

        /// <summary>
        /// Configures the <see cref="ExecutionContext"/>.
        /// </summary>
        /// <param name="tenantId"><see cref="TenantId"/> to use.</param>
        /// <param name="correlationId">The current <see cref="CorrelationId"/>.</param>
        /// <param name="claims">Current <see cref="Claims"/>.</param>
        /// <returns>The configured <see cref="ExecutionContext"/>.</returns>
        ExecutionContext ConfigureFor(TenantId tenantId, CorrelationId correlationId, Claims claims);
    }
}