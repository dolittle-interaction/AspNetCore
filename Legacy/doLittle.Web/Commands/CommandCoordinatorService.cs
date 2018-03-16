/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using Dolittle.Commands;
using Dolittle.Strings;
using Dolittle.Logging;
using Dolittle.Serialization;

namespace Dolittle.Web.Commands
{
    public class CommandCoordinatorService
    {
        readonly ICommandCoordinator _commandCoordinator;
        readonly ISerializer _serializer;
        readonly ILogger _logger;

        public CommandCoordinatorService(
            ICommandCoordinator commandCoordinator,
            ISerializer serializer,
            ILogger logger)
        {
            _commandCoordinator = commandCoordinator;
            _serializer = serializer;
            _logger = logger;
        }

        public CommandResult Handle(JsonCommandRequest command)
        {
            try
            {
                var contentAsKeyValues = _serializer.GetKeyValuesFromJson(command.Content).ToDictionary(k => k.Key.ToPascalCase(), k => k.Value);
                var commandRequest = new CommandRequest(command.CorrelationId, command.Type, contentAsKeyValues);

                var result = _commandCoordinator.Handle(commandRequest);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failed handling command '{command.Type}'");
                return new CommandResult { Exception = ex };
            }
        }

        public IEnumerable<CommandResult> HandleMany(IEnumerable<JsonCommandRequest> commands)
        {
            var results = new List<CommandResult>();
            foreach (var command in commands)
            {
                var contentAsKeyValues = _serializer.GetKeyValuesFromJson(command.Content).ToDictionary(k => k.Key.ToPascalCase(), k => k.Value);
                var commandRequest = new CommandRequest(command.CorrelationId, command.Type, contentAsKeyValues);
                try
                {
                    results.Add(_commandCoordinator.Handle(commandRequest));
                }
                catch (Exception ex)
                {
                    var commandResult = CommandResult.ForCommand(commandRequest);
                    commandResult.Exception = ex;
                    return new[] { commandResult };
                }
            }

            return results.ToArray();
        }
    }
}
