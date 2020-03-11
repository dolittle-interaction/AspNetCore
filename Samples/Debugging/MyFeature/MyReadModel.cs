// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.ReadModels;

namespace Debugging.MyFeature
{
    /// <summary>
    /// Represents a read model for testing.
    /// </summary>
    public class MyReadModel : IReadModel
    {
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        public int Number { get; set; } = 42;
    }
}