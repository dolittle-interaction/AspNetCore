/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.Artifacts;
using Dolittle.Reflection;
using Dolittle.Runtime.Commands;
using Dolittle.Serialization.Json;
using Dolittle.Strings;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dolittle.AspNetCore.Commands
{
    /// <summary>
    /// Represents a <see cref="IModelBinder"/> for binding <see cref="CommandRequest"/>
    /// </summary>
    public class CommandRequestBinder : IModelBinder
    {
        readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of <see cref="CommandRequestBinder"/>
        /// </summary>
        /// <param name="serializer"><see cref="ISerializer"/> to use</param>
        public CommandRequestBinder(ISerializer serializer)
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

                    var commandRequestKeyValues = _serializer.GetKeyValuesFromJson(json);
                    var content = _serializer.GetKeyValuesFromJson(commandRequestKeyValues["content"].ToString());

                    var commandRequest = new CommandRequest(
                        Guid.Parse(commandRequestKeyValues["correlationId"].ToString()),
                        Guid.Parse(commandRequestKeyValues["type"].ToString()),
                        ArtifactGeneration.First,
                        content.ToDictionary(keyValue => keyValue.Key.ToPascalCase(), keyValue => keyValue.Value)
                    );

                    bindingContext.Result = ModelBindingResult.Success(commandRequest);
                }

                bindingContext.HttpContext.Request.Body = buffer;
            }
        }
    }   
}
