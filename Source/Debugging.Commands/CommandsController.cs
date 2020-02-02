// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Artifacts;
using Dolittle.Commands;
using Dolittle.PropertyBags;
using Microsoft.AspNetCore.Mvc;

namespace Dolittle.AspNetCore.Debugging.Commands
{
    /// <summary>
    /// Represents a debugging API endpoint for working with <see cref="ICommand">commands</see>.
    /// </summary>
    [Route("api/Dolittle/Debugging/Commands")]
    public class CommandsController : ControllerBase
    {
        readonly IArtifactTypeMap _artifactTypeMap;
        readonly IObjectFactory _objectFactory;
        readonly ICommandCoordinator _coordinator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandsController"/> class.
        /// </summary>
        /// <param name="artifactTypeMap">A <see cref="IArtifactTypeMap"/> for mappnig between artifacts and types.</param>
        /// <param name="objectFactory">A <see cref="IObjectFactory"/> for creating instances of types.</param>
        /// <param name="coordinator">A <see cref="ICommandCoordinator"/> for coordinating commands.</param>
        public CommandsController(
            IArtifactTypeMap artifactTypeMap,
            IObjectFactory objectFactory,
            ICommandCoordinator coordinator)
        {
            _artifactTypeMap = artifactTypeMap;
            _objectFactory = objectFactory;
            _coordinator = coordinator;
        }

        /// <summary>
        /// [POST] Action for handling a command.
        /// </summary>
        /// <param name="request">The command and metadata to handle.</param>
        /// <returns><see cref="IActionResult"/> with result from handling the command.</returns>
        [HttpPost]
        public IActionResult Handle([FromBody] HandleCommandRequest request)
        {
            var type = _artifactTypeMap.GetTypeFor(request.Artifact);
            var command = _objectFactory.Build(type, request.Command) as ICommand;
            var result = _coordinator.Handle(request.Tenant, command);
            return Ok(result);
        }
    }
}