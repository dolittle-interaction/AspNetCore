// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.AspNetCore.Bootstrap;
using Microsoft.AspNetCore.Mvc;

namespace Dolittle.AspNetCore.Commands
{
    /// <summary>
    /// Augment <see cref="MvcOptions"/>.
    /// </summary>
    public class MvcOptionsAugmenter : ICanAddMvcOptions
    {
        /// <inheritdoc/>
        public void Add(MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new CommandRequestBinderProvider());
        }
    }
}