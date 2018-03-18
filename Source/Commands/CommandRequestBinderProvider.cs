/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Runtime.Commands;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Dolittle.AspNetCore.Commands
{
    /// <summary>
    /// Represents a <see cref="IModelBinderProvider"/> for providing <see cref="CommandRequestBinder"/>
    /// </summary>
    public class CommandRequestBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc/>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(CommandRequest))return new BinderTypeModelBinder(typeof(CommandRequestBinder));

            return null;
        }
    }
}
