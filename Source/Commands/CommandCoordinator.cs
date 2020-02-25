// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Dolittle.AspNetCore.Execution;
using Dolittle.Commands;
using Dolittle.Commands.Coordination.Runtime;
using Dolittle.Logging;
using Dolittle.Serialization.Json;
using Dolittle.Types;
using Microsoft.AspNetCore.Mvc;

namespace Dolittle.AspNetCore.Commands
{
    /// <summary>
    /// Represents an API endpoint for working with <see cref="ICommand">commands</see>.
    /// </summary>
    [Route("api/Dolittle/Commands")]
    public class CommandCoordinator : ControllerBase
    {
        readonly ICommandCoordinator _commandCoordinator;
        readonly IInstancesOf<ICommand> _commands;
        readonly ISerializer _serializer;
        readonly IExecutionContextConfigurator _executionContextConfigurator;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandCoordinator"/> class.
        /// </summary>
        /// <param name="commandCoordinator">The underlying <see cref="ICommandCoordinator"/>.</param>
        /// <param name="executionContextConfigurator"><see cref="IExecutionContextConfigurator"/> for configuring the <see cref="Dolittle.Execution.ExecutionContext"/>.</param>
        /// <param name="serializer"><see cref="ISerializer"/> for serialization purposes.</param>
        /// <param name="commands">Instances of <see cref="ICommand"/>.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public CommandCoordinator(
            ICommandCoordinator commandCoordinator,
            IExecutionContextConfigurator executionContextConfigurator,
            ISerializer serializer,
            IInstancesOf<ICommand> commands,
            ILogger logger)
        {
            _commandCoordinator = commandCoordinator;
            _commands = commands;
            _serializer = serializer;
            _executionContextConfigurator = executionContextConfigurator;
            _logger = logger;
        }

        /// <summary>
        /// [POST] Action for handling a <see cref="CommandRequest"/>.
        /// </summary>
        /// <param name="command"><see cref="CommandRequest"/> to handle.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public ActionResult Handle([FromBody] CommandRequest command)
        {
            var content = new ContentResult();
            CommandResult result;
            try
            {
                result = _commandCoordinator.Handle(command);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Could not handle command request");
                result = new CommandResult()
                {
                    Command = command,
                    Exception = ex,
                    ExceptionMessage = ex.Message
                };
            }

            content.Content = _serializer.ToJson(result, SerializationOptions.CamelCase);
            content.ContentType = "application/json";
            return content;
        }

        /// <summary>
        /// [GET] Action for getting all available commands.
        /// </summary>
        /// <returns>A collection of all implementations of <see cref="ICommand"/>.</returns>
        [HttpGet]
        public IEnumerable<ICommand> Commands()
        {
            try
            {
                return _commands.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error listing commands.");
                throw;
            }
        }
    }
}