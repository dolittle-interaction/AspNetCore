/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.DependencyInversion;

namespace Dolittle.AspNetCore.Bootstrap
{
    /// <summary>
    /// Represents an implementation of <see cref="IContainer"/> for <see cref="IServiceProvider"/>
    /// </summary>
    public class Container : IContainer
    {
        readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="Container"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
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