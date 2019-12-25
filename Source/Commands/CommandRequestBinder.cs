// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.Artifacts;
using Dolittle.Execution;
using Dolittle.Reflection;
using Dolittle.Runtime.Commands;
using Dolittle.Serialization.Json;
using Dolittle.Strings;
using Dolittle.Tenancy;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dolittle.AspNetCore.Commands
{
    /// <summary>
    /// Represents a <see cref="IModelBinder"/> for binding <see cref="CommandRequest"/>.
    /// </summary>
    public class CommandRequestBinder : IModelBinder
    {
        readonly ISerializer _serializer;
        readonly IExecutionContextManager _executionContextManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRequestBinder"/> class.
        /// </summary>
        /// <param name="serializer"><see cref="ISerializer"/> to use.</param>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> to use for working with the <see cref="ExecutionContext"/>.</param>
        public CommandRequestBinder(ISerializer serializer, IExecutionContextManager executionContextManager)
        {
            _serializer = serializer;
            _executionContextManager = executionContextManager;
        }

        /// <inheritdoc/>
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var stream = bindingContext.HttpContext.Request.Body;
            using (var buffer = new MemoryStream())
            {
                await stream.CopyToAsync(buffer).ConfigureAwait(false);

                buffer.Position = 0L;

                using (var reader = new StreamReader(buffer))
                {
                    var json = await reader.ReadToEndAsync().ConfigureAwait(false);

                    var commandRequestKeyValues = _serializer.GetKeyValuesFromJson(json);
                    var correlationId = Guid.Parse(commandRequestKeyValues["correlationId"].ToString());
                    _executionContextManager.CurrentFor(TenantId.Unknown, correlationId);

                    var content = _serializer.GetKeyValuesFromJson(commandRequestKeyValues["content"].ToString());

                    var commandRequest = new CommandRequest(
                        Guid.Parse(commandRequestKeyValues["correlationId"].ToString()),
                        Guid.Parse(commandRequestKeyValues["type"].ToString()),
                        ArtifactGeneration.First,
                        content.ToDictionary(keyValue => keyValue.Key.ToPascalCase(), keyValue => keyValue.Value));

                    bindingContext.Result = ModelBindingResult.Success(commandRequest);
                }

                bindingContext.HttpContext.Request.Body = buffer;
            }
        }
    }
}
