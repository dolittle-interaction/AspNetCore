// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Commands;
using Dolittle.Runtime.Commands;
using Dolittle.Tenancy;

namespace Dolittle.AspNetCore.Debugging.Commands
{
    /// <summary>
    /// Represents a coordinator capable of handling commands.
    /// </summary>
    public interface ICommandCoordinator
    {
        /// <summary>
        /// Handle a command.
        /// </summary>
        /// <param name="tenant">Current <see cref="TenantId"/> for handling.</param>
        /// <param name="command">The instance of the <see cref="ICommand"/> to handle.</param>
        /// <returns><see cref="CommandResult"/> from handling.</returns>
        CommandResult Handle(TenantId tenant, ICommand command);
    }
}