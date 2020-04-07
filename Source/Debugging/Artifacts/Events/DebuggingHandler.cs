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
    /// hi ther.
    /// </summary>
    public class DebuggingHandler : IDebuggingHandler, ICanHandlePostRequests<IEvent>
    {
        readonly IEventStore _eventStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggingHandler"/> class.
        /// nice.
        /// </summary>
        /// <param name="events">hie.</param>
        /// <param name="eventStore">nioe.</param>
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
        public string Title => "Apply Events";

        /// <inheritdoc/>
        public IDictionary<PathString, Type> Artifacts { get; } = new Dictionary<PathString, Type>();

        /// <inheritdoc/>
        public IDictionary<int, string> Responses => new Dictionary<int, string>
        {
            { StatusCodes.Status200OK, "The event uhhh ecxists i guess." },
            { StatusCodes.Status500InternalServerError, "The event fdk up." },
        };

        /// <inheritdoc/>
        public async Task HandlePostRequest(HttpContext context, IEvent artifact)
        {
            // add a IEventStore thingy to commit the artifact
            var uncommittedEvent = new UncommittedEvents();
            uncommittedEvent.Append(artifact);
            var committedEvents = _eventStore.Commit(uncommittedEvent);
            await context.RespondWithOk($"Event {artifact.GetType()} was here {artifact} {committedEvents}").ConfigureAwait(false);
        }
    }
}