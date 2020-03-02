// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Artifacts;
using Dolittle.Events;
using Dolittle.Execution;
using Dolittle.Logging;
using Dolittle.Tenancy;

namespace Dolittle.AspNetCore.Debugging.Events
{
    /// <summary>
    /// Represents an implementation of an <see cref="IEventInjector"/>.
    /// </summary>
    public class EventInjector : IEventInjector
    {
        readonly IExecutionContextManager _executionContextManager;
        readonly IArtifactTypeMap _artifactTypeMap;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventInjector"/> class.
        /// </summary>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with <see cref="ExecutionContext"/>.</param>
        /// <param name="artifactTypeMap"><see cref="IArtifactTypeMap"/> for mapping artifacts and types.</param>
        /// <param name="logger"><see cref="ILogger" /> for logging.</param>
        public EventInjector(
            IExecutionContextManager executionContextManager,
            IArtifactTypeMap artifactTypeMap,
            ILogger logger)
        {
            _executionContextManager = executionContextManager;
            _artifactTypeMap = artifactTypeMap;
            _logger = logger;
        }

        /// <inheritdoc/>
        public void InjectEvent(TenantId tenant, EventSourceId eventSourceId, IEvent @event)
        {
            _logger.Information($"Injecting event!");
        }
    }
}