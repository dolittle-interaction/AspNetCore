// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.Artifacts;
using Dolittle.DependencyInversion;
using Dolittle.Events;
using Dolittle.Execution;
using Dolittle.Logging;
using Dolittle.PropertyBags;
using Dolittle.Runtime.Events;
using Dolittle.Runtime.Events.Processing;
using Dolittle.Runtime.Events.Store;
using Dolittle.Tenancy;

namespace Dolittle.AspNetCore.Debugging.Events
{
    /// <summary>
    /// Represents an implementation of an <see cref="IEventInjector"/>.
    /// </summary>
    public class EventInjector : IEventInjector
    {
        readonly FactoryFor<IEventStore> _getEventStore;
        readonly IScopedEventProcessingHub _processingHub;
        readonly IExecutionContextManager _executionContextManager;
        readonly IArtifactTypeMap _artifactTypeMap;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventInjector"/> class.
        /// </summary>
        /// <param name="getEventStore"><see cref="FactoryFor{IEventStore}" /> factory function that returns a correctly scoped <see cref="IEventStore" />.</param>
        /// <param name="processingHub"><see cref="IScopedEventProcessingHub" /> for processing events from the <see cref="CommittedEventStream" />.</param>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with <see cref="ExecutionContext"/>.</param>
        /// <param name="artifactTypeMap"><see cref="IArtifactTypeMap"/> for mapping artifacts and types.</param>
        /// <param name="logger"><see cref="ILogger" /> for logging.</param>
        public EventInjector(
            FactoryFor<IEventStore> getEventStore,
            IScopedEventProcessingHub processingHub,
            IExecutionContextManager executionContextManager,
            IArtifactTypeMap artifactTypeMap,
            ILogger logger)
        {
            _getEventStore = getEventStore;
            _processingHub = processingHub;
            _executionContextManager = executionContextManager;
            _artifactTypeMap = artifactTypeMap;
            _logger = logger;
        }

        /// <inheritdoc/>
        public void InjectEvent(TenantId tenant, EventSourceId eventSourceId, IEvent @event)
        {
            _logger.Information($"Injecting event!");

            _executionContextManager.CurrentFor(tenant);
            var executionContext = _executionContextManager.Current;
            using (var eventStore = _getEventStore())
            {
                var artifact = _artifactTypeMap.GetArtifactFor(@event.GetType());
                var eventSourceKey = new EventSourceKey(eventSourceId, artifact.Id);
                var version = eventStore.GetNextVersionFor(eventSourceKey);

                var uncommittedEventStream = new UncommittedEventStream(
                    CommitId.New(),
                    executionContext.CorrelationId,
                    new VersionedEventSource(version, eventSourceKey),
                    DateTimeOffset.Now,
                    EventStream.From(new[]
                    {
                        new EventEnvelope(
                            new EventMetadata(
                                EventId.New(),
                                new VersionedEventSource(version, eventSourceKey),
                                executionContext.CorrelationId,
                                artifact,
                                DateTimeOffset.Now,
                                executionContext),
                            @event.ToPropertyBag())
                    }));

                _logger.Information("Commit events to store");
                var committedEventStream = eventStore.Commit(uncommittedEventStream);

                _logger.Information("Process committed events");
                _processingHub.Process(committedEventStream);
            }
        }
    }
}