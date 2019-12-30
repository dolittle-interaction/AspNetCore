// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.AspNetCore.Bootstrap;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Exception that gets thrown when a <see cref="ICanAddMvcOptions">MVC Options augmenter</see> does not have a default constructor.
    /// </summary>
    public class MissingDefaultConstructorForAugmenter : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingDefaultConstructorForAugmenter"/> class.
        /// </summary>
        /// <param name="type">Type of <see cref="ICanAddMvcOptions"/>.</param>
        public MissingDefaultConstructorForAugmenter(Type type)
            : base($"Augmenter of type '{type.AssemblyQualifiedName}' is missing a default constructor")
        {
        }
    }
}