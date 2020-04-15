// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Dolittle.AspNetCore.Debugging.Artifacts
{
    /// <summary>
    /// Represents a mapper that discovers artifacts by type, and maps them to a path.
    /// </summary>
    /// <typeparam name="T">Type of artifact.</typeparam>
    public interface IArtifactMapper<T>
        where T : class
    {
        /// <summary>
        /// Gets all discovered artifacts of type <typeparamref name="T"/>.
        /// </summary>
        IEnumerable<Type> Artifacts { get; }

        /// <summary>
        /// Gets the path that uniquely identifies an artifact.
        /// </summary>
        /// <param name="artifact">The <see cref="Type"/> representing the artifact.</param>
        /// <returns>The <see cref="PathString"/> that uniquely identifies an artifact. </returns>
        PathString GetPathFor(Type artifact);
    }
}