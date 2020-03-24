// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.Commands;

namespace Debugging.MyFeature
{
    /// <summary>
    /// hello.
    /// </summary>
    public class MyCommand : ICommand
    {
        /// <summary>
        /// Gets or sets.
        /// </summary>
        public Guid CommandId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// gets or sets the string.
        /// </summary>
        public string CommandString { get; set; }
    }
}