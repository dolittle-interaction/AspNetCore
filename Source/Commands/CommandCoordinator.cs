/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Dolittle.Commands;
using Dolittle.Runtime.Commands;
using Dolittle.Runtime.Commands.Coordination;
using Dolittle.Types;
using Microsoft.AspNetCore.Mvc;
using Dolittle.Serialization.Json;

namespace Dolittle.AspNetCore.Commands
{
    /// <summary>
    /// Represents an API endpoint for working with <see cref="ICommand">commands</see>
    /// </summary>
    [Route("api/Dolittle/Commands")]
    public class CommandCoordinator : ControllerBase
    {
        readonly ICommandCoordinator _commandCoordinator;
        readonly IInstancesOf<ICommand> _commands;
        readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of <see cref="CommandCoordinator"/>
        /// </summary>
        /// <param name="commandCoordinator">The underlying <see cref="ICommandCoordinator"/> </param>
        /// <param name="serializer"><see cref="ISerializer"/> for serialization purposes</param>
        /// <param name="commands">Instances of <see cref="ICommand"/></param>
        public CommandCoordinator(
            ICommandCoordinator commandCoordinator,
            ISerializer serializer,
            IInstancesOf<ICommand> commands)
        {
            _commandCoordinator = commandCoordinator;
            _commands = commands;
            _serializer = serializer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Handle([FromBody] CommandRequest command)
        {
            var result = _commandCoordinator.Handle(command);
            var content = new ContentResult();
            content.Content = _serializer.ToJson(result, SerializationOptions.CamelCase);
            content.ContentType = "application/json";
            return content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ICommand> Commands()
        {
            return _commands;
        }

    }
}