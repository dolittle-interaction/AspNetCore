// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Dolittle.Commands;

namespace Debugging.MyFeature
{
    /// <summary>
    /// Example Command.
    /// </summary>
    public class MyCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the CommandId.
        /// </summary>
        public CommandId CommandId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the CommandString.
        /// </summary>
        public IEnumerable<CommandString> CommandStrings { get; set; }

        /// <summary>
        /// Gets or sets the CommandInt.
        /// </summary>
        public int CommandInt { get; set; }
    }
}