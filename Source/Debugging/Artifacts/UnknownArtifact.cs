// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.AspNetCore.Debugging.Artifacts
{
    /// <summary>
    /// Exception that gets thrown when trying to get the path to an artifact that is not known.
    /// </summary>
    public class UnknownArtifact : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownArtifact"/> class.
        /// </summary>
        /// <param name="artifact">The <see cref="Type"/> that was tried to map to a path.</param>
        /// <param name="type">The base <see cref="Type"/> of the artifact.</param>
        public UnknownArtifact(Type artifact, Type type)
            : base($"The type {artifact} is not a known artifact of type {type}")
        {
        }
    }
}