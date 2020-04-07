// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Events;

namespace Debugging.MyFeature
{
    /// <summary>
    /// Example Event.
    /// </summary>
    public class MyEvent : IEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyEvent"/> class.
        ///  my construgs.
        /// </summary>
        /// <param name="myString">muh string.</param>
        public MyEvent(string myString)
        {
            MyString = myString;
        }

        /// <summary>
        /// Gets MyString.
        /// </summary>
        public string MyString { get; }
    }
}
