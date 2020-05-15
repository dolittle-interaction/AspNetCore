// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dolittle.AspNetCore.Debugging.Handlers;
using Dolittle.Events;
using Microsoft.AspNetCore.Http;

namespace Dolittle.AspNetCore.Debugging.Artifacts.Events
{
    /// <summary>
    /// Represents an implementation of <see cref="IDebuggingHandler" /> and <see cref="ICanHandleGetRequests{T}" /> for <see cref="IEvent" />.
    /// </summary>
    public class DebuggingHandler : IDebuggingHandler, ICanHandlePostRequests<IEvent>
    {
        readonly IEventStore _eventStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggingHandler"/> class.
        /// </summary>
        /// <param name="events">The <see cref="IArtifactMapper{T}" /> for <see cref="IEvent" />.</param>
        /// <param name="eventStore">The <see cref="IEventStore" />..</param>
        public DebuggingHandler(IArtifactMapper<IEvent> events, IEventStore eventStore)
        {
            _eventStore = eventStore;

            foreach (var @event in events.Artifacts)
            {
                Artifacts.Add(events.GetPathFor(@event), @event);
            }
        }

        /// <inheritdoc/>
        public string Name => "Events";

        /// <inheritdoc/>
        public string Title => "Commit Events";

        /// <inheritdoc/>
        public IDictionary<PathString, Type> Artifacts { get; } = new Dictionary<PathString, Type>();

        /// <inheritdoc/>
        public IDictionary<int, string> Responses => new Dictionary<int, string>
        {
            { StatusCodes.Status200OK, "Event committed successfully." },
            { StatusCodes.Status500InternalServerError, "Event  wasn't committed successfully." },
        };

        /// <inheritdoc/>
        public async Task HandlePostRequest(HttpContext context, IEvent @event)
        {
            var uncommittedEvents = new UncommittedEvents();
            uncommittedEvents.Append(EventSourceId.New(), @event);
            var committedEvents = await _eventStore.Commit(uncommittedEvents).ConfigureAwait(false);
            await context.RespondWithOk($"Event {@event.GetType()} committed. \nCommittedEvents: {committedEvents}").ConfigureAwait(false);
        }
    }
}