// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dolittle.AspNetCore.Debugging.Handlers;
using Dolittle.Commands;
using Dolittle.Commands.Coordination;
using Microsoft.AspNetCore.Http;

namespace Dolittle.AspNetCore.Debugging.Artifacts.Commands
{
    /// <summary>
    /// An implementation of <see cref="IDebuggingHandler"/> that executes an <see cref="ICommand"/>.
    /// </summary>
    public class DebuggingHandler : IDebuggingHandler, ICanHandlePostRequests<ICommand>
    {
        readonly IArtifactMapper<ICommand> _commands;
        readonly ICommandCoordinator _commandCoordinator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggingHandler"/> class.
        /// </summary>
        /// <param name="commands">The <see cref="IArtifactMapper{ICommand}"/> that discovers and maps all <see cref="ICommand"/>.</param>
        /// <param name="commandCoordinator">write somethingehre.</param>
        public DebuggingHandler(IArtifactMapper<ICommand> commands, ICommandCoordinator commandCoordinator)
        {
            _commands = commands;
            _commandCoordinator = commandCoordinator;

            foreach (var command in _commands.Artifacts)
            {
                Artifacts.Add(_commands.GetPathFor(command), command);
            }
        }

        /// <inheritdoc/>
        public string Name => "Commands";

        /// <inheritdoc/>
        public string Title => "Handle Commands";

        /// <inheritdoc/>
        public IDictionary<PathString, Type> Artifacts { get; } = new Dictionary<PathString, Type>();

        /// <inheritdoc/>
        public IDictionary<int, string> Responses => new Dictionary<int, string>
        {
            { StatusCodes.Status200OK, "Command handled succesfully." },
            { StatusCodes.Status500InternalServerError, "Command wasn't handled succefully." },
        };

        /// <inheritdoc/>
        public async Task HandlePostRequest(HttpContext context, ICommand artifact)
        {
            var commandResult = _commandCoordinator.Handle(artifact);
            if (commandResult.Success)
            {
                await context.RespondWithOk($"Command {artifact.GetType()} was handled successfully. \nCommandResult: {commandResult}").ConfigureAwait(false);
            }
            else
            {
                await context.RespondWithError($"Command {artifact.GetType()} wasn't handled succesfully. \nCommandResult: {commandResult}").ConfigureAwait(false);
            }
        }
    }
}
