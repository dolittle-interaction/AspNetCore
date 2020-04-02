// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.Concepts;
using Dolittle.Events;

namespace Debugging.MyFeature
{
    /// <summary>
    /// Example of a concept used to convert from Guid's to CommandID's.
    /// </summary>
    public class CommandId : ConceptAs<Guid>
    {
        /// <summary>
        /// Guid to CommandId.
        /// </summary>
        /// <param name="commandId">Guid.</param>
        public static implicit operator CommandId(Guid commandId) => new CommandId { Value = commandId };

        /// <summary>
        /// CommandId to EventSourceId.
        /// </summary>
        /// <param name="commandId">CommandId.</param>
        public static implicit operator EventSourceId(CommandId commandId) => commandId.Value;

        /// <summary>
        /// EventSourceId to CommandId.
        /// </summary>
        /// <param name="eventSourceId">EventSourceIde.</param>
        public static implicit operator CommandId(EventSourceId eventSourceId) => new CommandId { Value = eventSourceId.Value };
    }
}