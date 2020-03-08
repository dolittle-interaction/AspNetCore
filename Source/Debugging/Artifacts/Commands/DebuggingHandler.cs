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
        /// <inheritdoc/>
        public string Name => "Commands";

        /// <inheritdoc/>
        public string Title => "Execute Commands";

        /// <inheritdoc/>
        public IDictionary<PathString, Type> Aritfacts => new Dictionary<PathString, Type>();

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