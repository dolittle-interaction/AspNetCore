/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Dolittle.Applications;
using Dolittle.CodeGeneration;
using Dolittle.CodeGeneration.JavaScript;
using Dolittle.Commands;
using Dolittle.Execution;
using Dolittle.Strings;
using Dolittle.Types;

namespace Dolittle.AspNetCore.Commands.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommandProxies
    {
        /// <summary>
        /// 
        /// </summary>
        string Generate();

    }
}
