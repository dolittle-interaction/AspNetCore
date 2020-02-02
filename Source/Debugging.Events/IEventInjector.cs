// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Events;
using Dolittle.Runtime.Events;
using Dolittle.Tenancy;

namespace Dolittle.AspNetCore.Debugging.Events
{
    /// <summary>
    /// Represents an injector capable of inserting events directly into the event store and trigger event processors.
    /// </summary>
    public interface IEventInjector
    {
        /// <summary>
        /// Injects an event.
        /// </summary>
        /// <param name="tenant">The <see cref="TenantId"/> we're running in context of.</param>
        /// <param name="eventSourceId"><see cref="EventSourceId"/> for injecting on behalf of.</param>
        /// <param name="event">The instance of the <see cref="IEvent"/> being injected.</param>
        void InjectEvent(TenantId tenant, EventSourceId eventSourceId, IEvent @event);
    }
}