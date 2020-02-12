// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Artifacts;
using Dolittle.PropertyBags;
using Dolittle.Runtime.Events;
using Dolittle.Tenancy;

namespace Dolittle.AspNetCore.Debugging.Events
{
    /// <summary>
    /// Captures the information necessary to inject an event into the event store.
    /// </summary>
    public class InjectEventRequest
    {
        /// <summary>
        /// Gets or sets the tenant to inject the event into.
        /// </summary>
        public TenantId Tenant { get; set; }

        /// <summary>
        /// Gets or sets the artifact identifying the event.
        /// </summary>
        public Artifact Artifact { get; set; }

        /// <summary>
        /// Gets or sets the event source to apply the event to.
        /// </summary>
        public EventSourceId EventSource { get; set; }

        /// <summary>
        /// Gets or sets the actual event data.
        /// </summary>
        public PropertyBag Event { get; set; }
    }
}