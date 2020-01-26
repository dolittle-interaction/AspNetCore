// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.DependencyInversion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents an <see cref="ICanProvideBindings"/> instance for providing Dolittle AspNetCore bootstrapping bindings.
    /// </summary>
    public class Bindings : ICanProvideBindings
    {
        /// <inheritdoc/>
        public void Provide(IBindingProviderBuilder builder)
        {
            builder.Bind<IConfigureOptions<MvcNewtonsoftJsonOptions>>().To<MvcNewtonsoftJsonConverters>();
        }
    }
}