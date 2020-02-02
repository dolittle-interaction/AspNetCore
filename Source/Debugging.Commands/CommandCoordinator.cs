// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Commands;
using Dolittle.Execution;
using Dolittle.Runtime.Commands;
using Dolittle.Tenancy;
using IRuntimeCommandCoordinator = Dolittle.Commands.Coordination.ICommandCoordinator;

namespace Dolittle.AspNetCore.Debugging.Commands
{
    /// <summary>
    /// An implementation of <see cref="ICommandCoordinator"/>.
    /// </summary>
    public class CommandCoordinator : ICommandCoordinator
    {
        readonly IExecutionContextManager _executionContextManager;
        readonly IRuntimeCommandCoordinator _runtimeCommandCoordinator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandCoordinator"/> class.
        /// </summary>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with <see cref="ExecutionContext"/>.</param>
        /// <param name="runtimeCommandCoordinator"><see cref="IRuntimeCommandCoordinator"/> for handling commands.</param>
        public CommandCoordinator(
            IExecutionContextManager executionContextManager,
            IRuntimeCommandCoordinator runtimeCommandCoordinator)
        {
            _executionContextManager = executionContextManager;
            _runtimeCommandCoordinator = runtimeCommandCoordinator;
        }

        /// <inheritdoc/>
        public CommandResult Handle(TenantId tenant, ICommand command)
        {
            _executionContextManager.CurrentFor(tenant);
            return _runtimeCommandCoordinator.Handle(command);
        }
    }
}