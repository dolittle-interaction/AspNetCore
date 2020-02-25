// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dolittle.AspNetCore.Debugging.Swagger.Artifacts;
using Dolittle.Concepts;
using Dolittle.Logging;
using Dolittle.Serialization.Json;
using Dolittle.Tenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Dolittle.AspNetCore.Debugging.Swagger
{
    /// <summary>
    /// Represents a controller that handles artifacts by fully qualified name encoded in the path.
    /// </summary>
    /// <typeparam name="T">Type of artifact to handle.</typeparam>
    public abstract class ArtifactControllerBase<T> : ControllerBase
        where T : class
    {
        readonly ILogger _logger;
        readonly IArtifactMapper<T> _artifactTypes;
        readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtifactControllerBase{T}"/> class.
        /// </summary>
        /// <param name="artifactTypes"><see cref="IArtifactMapper{T}"/> for mapping artifacts.</param>
        /// <param name="serializer">The JSON <see cref="ISerializer"/> for deserializing artifacts.</param>
        /// <param name="logger">The <see cref="ILogger"/> to use.</param>
        protected ArtifactControllerBase(
            IArtifactMapper<T> artifactTypes,
            ISerializer serializer,
            ILogger logger)
        {
            _artifactTypes = artifactTypes;
            _serializer = serializer;
            _logger = logger;
        }

        /// <summary>
        /// Tries to resolve tenant and artifact based on the incoming request.
        /// </summary>
        /// <param name="path">The request path.</param>
        /// <param name="request">The request data.</param>
        /// <param name="tenant">The resolved tenant.</param>
        /// <param name="artifact">The created artifact.</param>
        /// <returns>Whether the resolution worked.</returns>
        protected bool TryResolveTenantAndArtifact(string path, IDictionary<string, StringValues> request, out TenantId tenant, out T artifact)
        {
            if (path[0] != '/') path = $"/{path}";

            var artifactType = _artifactTypes.GetTypeFor(path);
            tenant = request["TenantId"][0].ParseTo(typeof(TenantId)) as TenantId;

            var requestAsJson = JsonConvert.SerializeObject(request);
            artifact = _serializer.FromJson(artifactType, requestAsJson) as T;
            return artifact != null;
        }
    }
}