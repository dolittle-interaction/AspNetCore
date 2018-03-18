/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.IO;
using System.Threading.Tasks;
using Dolittle.Serialization.Json;
using Dolittle.AspNetCore.Queries;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dolittle.AspNetCore.Queries
{
    /// <summary>
    /// Represents a <see cref="IModelBinder"/> for binding <see cref="QueryRequest"/>
    /// </summary>
    public class QueryRequestBinder : IModelBinder
    {
        readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of <see cref="QueryRequestBinder"/>
        /// </summary>
        /// <param name="serializer"></param>
        public QueryRequestBinder(ISerializer serializer)
        {
            _serializer = serializer;
        }

        /// <inheritdoc/>
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var stream = bindingContext.HttpContext.Request.Body;

            using(var buffer = new MemoryStream())
            {
                await stream.CopyToAsync(buffer);

                buffer.Position = 0L;

                using(var reader = new StreamReader(buffer))
                {
                    var json = await reader.ReadToEndAsync();
                    var commandRequest = _serializer.FromJson<QueryRequest>(json);
                    bindingContext.Result = ModelBindingResult.Success(commandRequest);
                }

                bindingContext.HttpContext.Request.Body = buffer;
            }
        }
    }

}