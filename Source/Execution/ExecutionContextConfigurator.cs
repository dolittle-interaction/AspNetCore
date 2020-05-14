// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.ApplicationModel;
using Dolittle.Execution;
using Dolittle.Lifecycle;
using Dolittle.Security;
using Dolittle.Tenancy;
using Dolittle.Versioning;

namespace Dolittle.AspNetCore.Execution
{
    /// <summary>
    /// Represents an implementation of <see cref="IExecutionContextConfigurator"/>.
    /// </summary>
    [Singleton]
    public class ExecutionContextConfigurator : IExecutionContextConfigurator
    {
        readonly IExecutionContextManager _executionContextManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionContextConfigurator"/> class.
        /// </summary>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with <see cref="ExecutionContext"/>.</param>
        public ExecutionContextConfigurator(IExecutionContextManager executionContextManager)
        {
            _executionContextManager = executionContextManager;
        }

        /// <inheritdoc/>
        public ExecutionContext CurrentExecutionContext()
        {
            return _executionContextManager.Current;
        }

        /// <inheritdoc/>
        public ExecutionContext ConfigureFor(TenantId tenantId, CorrelationId correlationId, Claims claims)
        {
            _executionContextManager.SetConstants(CurrentMicroservice(), CurrentVersion(), EnvironmentUtilities.GetExecutionEnvironment());

            return _executionContextManager.CurrentFor(tenantId, correlationId, claims ?? Claims.Empty);
        }

        Microservice CurrentMicroservice() => _executionContextManager.Current.Microservice;

        Version CurrentVersion() => _executionContextManager.Current.Version;
    }
}
