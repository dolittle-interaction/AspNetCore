// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Dolittle.Collections;
using Dolittle.Serialization.Json;
using Dolittle.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// An options configurer for <see cref="MvcNewtonsoftJsonOptions"/> that adds custom instances of <see cref="JsonConverter"/> for use in MVC.
    /// </summary>
    public class MvcNewtonsoftJsonConverters : IConfigureOptions<MvcNewtonsoftJsonOptions>
    {
        readonly IInstancesOf<ICanProvideConverters> _providers;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcNewtonsoftJsonConverters"/> class.
        /// </summary>
        /// <param name="providers">All <see cref="IInstancesOf{T}"/> <see cref="ICanProvideConverters"/>.</param>
        public MvcNewtonsoftJsonConverters(IInstancesOf<ICanProvideConverters> providers)
        {
            _providers = providers;
        }

        /// <inheritdoc/>
        public void Configure(MvcNewtonsoftJsonOptions options)
        {
            var converters = _providers.SelectMany(_ => _.Provide());
            converters.ForEach(options.SerializerSettings.Converters.Add);
        }
    }
}