// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Threading.Tasks;
using Dolittle.Artifacts;
using Dolittle.PropertyBags;
using Dolittle.Serialization.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dolittle.AspNetCore.Debugging.Events
{
    /// <summary>
    /// Represents a <see cref="IModelBinder"/> for binding <see cref="InjectEventRequest"/>.
    /// </summary>
    public class InjectEventRequestBinder : IModelBinder
    {
        readonly ISerializer _serializer;
        readonly IArtifactTypeMap _artifactTypeMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="InjectEventRequestBinder"/> class.
        /// </summary>
        /// <param name="serializer"><see cref="ISerializer"/> to use.</param>
        /// <param name="artifactTypeMap"><see cref="IArtifactTypeMap"/> for mapping artifacts and types.</param>
        public InjectEventRequestBinder(ISerializer serializer, IArtifactTypeMap artifactTypeMap)
        {
            _serializer = serializer;
            _artifactTypeMap = artifactTypeMap;
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
                    var requestKeyValues = _serializer.GetKeyValuesFromJson(json);
                    var request = new InjectEventRequest
                    {
                        Tenant = Guid.Parse(requestKeyValues["tenant"].ToString()),
                        Artifact = _serializer.FromJson<Artifact>(requestKeyValues["artifact"].ToString()),
                        EventSource = Guid.Parse(requestKeyValues["eventSource"].ToString()),
                    };

                    var eventType = _artifactTypeMap.GetTypeFor(request.Artifact);
                    var eventData = _serializer.FromJson(eventType, requestKeyValues["event"].ToString());
                    request.Event = eventData.ToPropertyBag();

                    bindingContext.Result = ModelBindingResult.Success(request);
                }

                bindingContext.HttpContext.Request.Body = buffer;
            }
        }
    }
}