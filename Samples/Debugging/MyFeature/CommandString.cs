// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Concepts;

namespace Debugging.MyFeature
{
    /// <summary>
    /// Example concept.
    /// </summary>
    public class CommandString : ConceptAs<string>
    {
        /// <summary>
        /// Example of a string concept.
        /// </summary>
        /// <param name="commandString">string.</param>
        public static implicit operator CommandString(string commandString) => commandString + " is a CommandString";
    }
}