// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Domain;
using Dolittle.Events;

namespace Debugging.MyFeature
{
    /// <summary>
    /// k.
    /// </summary>
    public class MyAggregateRoot : AggregateRoot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyAggregateRoot"/> class.
        /// </summary>
        /// <param name="eventSourceId">cool id.</param>
        public MyAggregateRoot(EventSourceId eventSourceId)
            : base(eventSourceId)
        {
        }

        /// <summary>
        /// i do that thing.
        /// </summary>
        /// <param name="myString">this string is da thing.</param>
        public void DoThing(string myString)
        {
            Apply(new MyEvent(myString));
        }
    }
}
