// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Commands.Handling;

namespace Debugging.MyFeature
{
    /// <summary>
    /// Represents a command handler for testing.
    /// </summary>
    public class MyCommandHandler : ICanHandleCommands
    {
        /// <summary>
        /// Handles a <see cref="MyCommand"/>.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Handle(MyCommand command)
        {
        }
    }
}