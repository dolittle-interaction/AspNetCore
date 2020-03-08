// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dolittle.AspNetCore.Debugging.Handlers;
using Dolittle.Commands;
using Microsoft.AspNetCore.Http;

namespace Dolittle.AspNetCore.Debugging.Artifacts.Commands
{
    /// <summary>
    /// An implementation of <see cref="IDebuggingHandler"/> that executes an <see cref="ICommand"/>.
    /// </summary>
    public class DebuggingHandler : IDebuggingHandler, ICanHandlePostRequests<ICommand>
    {
        readonly IArtifactMapper<ICommand> _commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggingHandler"/> class.
        /// </summary>
        /// <param name="commands">The <see cref="IArtifactMapper{ICommand}"/> that discovers and maps all <see cref="ICommand"/>.</param>
        public DebuggingHandler(IArtifactMapper<ICommand> commands)
        {
            _commands = commands;

            foreach (var command in _commands.Artifacts)
            {
                Aritfacts.Add(_commands.GetPathFor(command), command);
            }
        }

        /// <inheritdoc/>
        public string Name => "Commands";

        /// <inheritdoc/>
        public string Title => "Execute Commands";

        /// <inheritdoc/>
        public IDictionary<PathString, Type> Aritfacts { get; } = new Dictionary<PathString, Type>();

        /// <inheritdoc/>
        public IDictionary<int, string> Responses => new Dictionary<int, string>
        {
            { StatusCodes.Status200OK, "The command was handled successfully." },
        };

        /// <inheritdoc/>
        public async Task HandlePostRequest(HttpContext context, ICommand artifact)
        {
            await context.RespondWithOk($"Command {artifact.GetType()} was handled successfully.").ConfigureAwait(false);
        }
    }
}