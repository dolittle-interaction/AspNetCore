// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Artifacts;
using Dolittle.PropertyBags;
using Dolittle.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Dolittle.AspNetCore.Debugging.Queries
{
    /// <summary>
    /// Represents a debugging API endpoint for working with <see cref="IQuery">queries</see>.
    /// </summary>
    [Route("api/Dolittle/Debugging/Queries")]
    public class QueriesController : ControllerBase
    {
        readonly IArtifactTypeMap _artifactTypeMap;
        readonly IObjectFactory _objectFactory;
        readonly IQueryCoordinator _coordinator;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueriesController"/> class.
        /// </summary>
        /// <param name="artifactTypeMap"><see cref="IArtifactTypeMap"/> for mapping artifacts and types.</param>
        /// <param name="objectFactory"><see cref="IObjectFactory"/> for creating instances of types.</param>
        /// <param name="coordinator"><see cref="IQueryCoordinator"/> for executing queries.</param>
        public QueriesController(
            IArtifactTypeMap artifactTypeMap,
            IObjectFactory objectFactory,
            IQueryCoordinator coordinator)
        {
            _artifactTypeMap = artifactTypeMap;
            _objectFactory = objectFactory;
            _coordinator = coordinator;
        }

        /// <summary>
        /// [POST] Action for executing a query.
        /// </summary>
        /// <param name="request">The query and metadata to execute.</param>
        /// <returns><see cref="IActionResult"/> with the result of executing the query.</returns>
        [HttpPost]
        public IActionResult Execute([FromBody] ExecuteQueryRequest request)
        {
            var type = _artifactTypeMap.GetTypeFor(request.Artifact);
            var command = _objectFactory.Build(type, request.Query) as IQuery;
            var result = _coordinator.Execute(request.Tenant, command);
            return Ok(result);
        }
    }
}