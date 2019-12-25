// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.DependencyInversion;

namespace Dolittle.AspNetCore.Bootstrap
{
    /// <summary>
    /// Represents an implementation of <see cref="IContainer"/> for <see cref="IServiceProvider"/>.
    /// </summary>
    public class Container : IContainer
    {
        readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Container"/> class.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
        public Container(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public T Get<T>()
        {
            return (T)_serviceProvider.GetService(typeof(T));
        }

        /// <inheritdoc/>
        public object Get(Type type)
        {
            return _serviceProvider.GetService(type);
        }
    }
}