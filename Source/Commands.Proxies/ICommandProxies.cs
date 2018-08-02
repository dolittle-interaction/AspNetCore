/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Commands;

namespace Dolittle.AspNetCore.Commands.Proxies
{
    /// <summary>
    /// Defines the proxy generation for <see cref="ICommand">commands</see>
    /// </summary>
    public interface ICommandProxies
    {
        /// <summary>
        /// Generate proxies for <see cref="ICommand">commands</see> in the running process
        /// </summary>
        string Generate();
    }
}
