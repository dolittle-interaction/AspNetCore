/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Dolittle.AspNetCore.Execution;
using Dolittle.Commands;
using Dolittle.Logging;
using Dolittle.Runtime.Commands;
using Dolittle.Runtime.Commands.Coordination;
using Dolittle.Security;
using Dolittle.Serialization.Json;
using Dolittle.Tenancy;
using Dolittle.Types;
using Microsoft.AspNetCore.Mvc;

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
        readonly IExecutionContextConfigurator _executionContextConfigurator;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="CommandCoordinator"/>
        /// </summary>
        /// <param name="commandCoordinator">The underlying <see cref="ICommandCoordinator"/> </param>
        /// <param name="executionContextConfigurator"><see cref="IExecutionContextConfigurator"/> for configuring the <see cref="Dolittle.Execution.ExecutionContext"/></param>
        /// <param name="serializer"><see cref="ISerializer"/> for serialization purposes</param>
        /// <param name="commands">Instances of <see cref="ICommand"/></param>
        /// <param name="logger"></param>
        public CommandCoordinator(
            ICommandCoordinator commandCoordinator,
            IExecutionContextConfigurator executionContextConfigurator,
            ISerializer serializer,
            IInstancesOf<ICommand> commands,
            ILogger logger
            )
        {
            _commandCoordinator = commandCoordinator;
            _commands = commands;
            _serializer = serializer;
            _executionContextConfigurator = executionContextConfigurator;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Handle([FromBody] CommandRequest command)
        {
            var content = new ContentResult();
            CommandResult result = null;
            try 
            {
                // _executionContextConfigurator.ConfigureFor(_tenantResolver.Resolve(HttpContext.Request), command.CorrelationId, ClaimsPrincipal.Current.ToClaims());
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ICommand> Commands()
        {
            try
            {
                // _executionContextConfigurator.ConfigureFor(_tenantResolver.Resolve(HttpContext.Request), Dolittle.Execution.CorrelationId.New(), ClaimsPrincipal.Current.ToClaims());
                return _commands.ToList();
            }
            catch(Exception ex)
            {

                _logger.Error(ex, $"Error listing commands.");
                throw;
            }
        }

    }
}