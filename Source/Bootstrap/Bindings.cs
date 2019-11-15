/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using Dolittle.DependencyInversion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents an <see cref="ICanProvideBindings"/> instance for providing Dolittle AspNetCore bootstrapping bindings
    /// </summary>
    public class Bindings : ICanProvideBindings
    {
        /// <summary>
        /// Provides Dolittle AspNetCore bootstrapping bindings
        /// </summary>
        /// <param name="builder"></param>
        public void Provide(IBindingProviderBuilder builder)
        {
            builder.Bind<IConfigureOptions<MvcJsonOptions>>().To<MvcJsonConverters>();
        }
    }
}