/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using Dolittle.Applications;
using Dolittle.Execution;
using Dolittle.Security;
using Dolittle.Tenancy;

namespace Dolittle.AspNetCore.Execution
{
    /// <summary>
    /// A system that configures the <see cref="ExecutionContext"/>
    /// </summary>
    public interface IExecutionContextConfigurator
    {
        /// <summary>
        /// Gets the current <see cref="ExecutionContext"/> from the <see cref="IExecutionContextManager"/>
        /// </summary>
        /// <returns></returns>
        ExecutionContext CurrentExecutionContext();
        /// <summary>
        /// Configures the <see cref="ExecutionContext"/>
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="correlationId"></param>
        /// <param name="claims"></param>
        /// <returns>The configured <see cref="ExecutionContext"/></returns>
        ExecutionContext ConfigureFor(TenantId tenantId, CorrelationId correlationId, Claims claims);
    }
}