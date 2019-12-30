// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Dolittle.AspNetCore.Queries
{
    /// <summary>
    /// Represents a <see cref="IModelBinderProvider"/> for <see cref="QueryRequestBinder"/>.
    /// </summary>
    public class QueryRequestBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc/>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(QueryRequest)) return new BinderTypeModelBinder(typeof(QueryRequestBinder));

            return null;
        }
    }
}
