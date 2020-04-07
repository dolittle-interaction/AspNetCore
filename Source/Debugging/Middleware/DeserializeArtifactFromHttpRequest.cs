// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;
using Dolittle.Concepts;
using Dolittle.Serialization.Json;
using Microsoft.AspNetCore.Http;

namespace Dolittle.AspNetCore.Debugging.Middleware
{
    /// <summary>
    /// An implementation of <see cref="IDeserializeArtifactFromHttpRequest"/> that deserializes JSON encoded artifacts from the <see cref="HttpRequest"/> body.
    /// </summary>
    public class DeserializeArtifactFromHttpRequest : IDeserializeArtifactFromHttpRequest
    {
        readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeserializeArtifactFromHttpRequest"/> class.
        /// </summary>
        /// <param name="serializer">The <see cref="ISerializer"/> used to deserialize the artifact.</param>
        public DeserializeArtifactFromHttpRequest(ISerializer serializer)
        {
            _serializer = serializer;
        }

        /// <inheritdoc/>
        public async Task<object> DeserializeArtifact(HttpRequest request, Type type)
        {
            try
            {
                if (request.HasFormContentType)
                {
                    // check the contenttype and serialize it accordingly aka get the activator done
                    // run the foreach on iformcollection and convert the strinvalues to the correct type.
                    // aka guid parse, using concepts then serialize to conecpt, if object then just do nothign basically
                    // for ienubmerable do foreach. strings guids floats concepts and ienumerables
                    var newArtifact = Activator.CreateInstance(type);
                    foreach (var artifactProperty in newArtifact.GetType().GetProperties())
                    {
                        var name = artifactProperty.Name;
                        foreach (var form in request.Form)
                        {
                            if (form.Key.Equals(name, StringComparison.OrdinalIgnoreCase))
                            {
                                // cast the thing into correct type
                                if (artifactProperty.PropertyType.BaseType.GetGenericTypeDefinition().IsAssignableFrom(typeof(ConceptAs<>)))
                                {
                                    // convert into concept
                                    var targetType = artifactProperty.PropertyType.BaseType;
                                    var converted = Convert.ChangeType((dynamic)form.Value, targetType);
                                    artifactProperty.SetValue(newArtifact, form.Value);
                                }
                            }
                        }
                    }

                    System.Console.WriteLine("oh shietetet");
                }

                var json = await ReadBodyAsString(request.BodyReader).ConfigureAwait(false);
                return _serializer.FromJson(type, json);
            }
            catch (Exception exception)
            {
                throw new ArtifactDeserializationFailed(type, exception);
            }
        }

        async Task<string> ReadBodyAsString(PipeReader reader)
        {
            ReadResult result;
            while (true)
            {
                result = await reader.ReadAsync().ConfigureAwait(false);
                if (result.IsCompleted) break;
                reader.AdvanceTo(result.Buffer.Start, result.Buffer.End);
            }

            var body = Encoding.UTF8.GetString(result.Buffer.ToArray());
            reader.AdvanceTo(result.Buffer.End);
            return body;
        }
    }
}