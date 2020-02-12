// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Artifacts;
using Dolittle.Events;
using Dolittle.PropertyBags;
using Microsoft.AspNetCore.Mvc;

namespace Dolittle.AspNetCore.Debugging.Events
{
    /// <summary>
    /// Represents a debugging API endpoint for working with <see cref="IEvent">events</see>.
    /// </summary>
    [Route("api/Dolittle/Debugging/Events")]
    public class EventInjectorController : ControllerBase
    {
        readonly IEventInjector _injector;
        readonly IObjectFactory _objectFactory;
        readonly IArtifactTypeMap _artifactTypeMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventInjectorController"/> class.
        /// </summary>
        /// <param name="injector">The underlying <see cref="IEventInjector"/>.</param>
        /// <param name="objectFactory"><see cref="IObjectFactory"/> for creating instances of types.</param>
        /// <param name="artifactTypeMap"><see cref="IArtifactTypeMap"/> for mapping artifacts and types.</param>
        public EventInjectorController(IEventInjector injector, IObjectFactory objectFactory, IArtifactTypeMap artifactTypeMap)
        {
            _injector = injector;
            _objectFactory = objectFactory;
            _artifactTypeMap = artifactTypeMap;
        }

        /// <summary>
        /// [POST] Action for injecting a new event into the event store and triggers event processors.
        /// </summary>
        /// <param name="request">The event and metadata to inject.</param>
        /// <returns><see cref="ActionResult"/> with result of injecting.</returns>
        [HttpPost]
        public ActionResult Inject([FromBody] InjectEventRequest request)
        {
            var type = _artifactTypeMap.GetTypeFor(request.Artifact);
            var @event = _objectFactory.Build(type, request.Event) as IEvent;

            _injector.InjectEvent(
                request.Tenant,
                request.EventSource,
                @event);

            return Ok();
        }
    }
}