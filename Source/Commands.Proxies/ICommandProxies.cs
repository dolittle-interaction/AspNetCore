/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 doLittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using doLittle.Applications;
using doLittle.CodeGeneration;
using doLittle.CodeGeneration.JavaScript;
using doLittle.Commands;
using doLittle.Execution;
using doLittle.Strings;
using doLittle.Types;

namespace doLittle.AspNetCore.Commands.Proxies
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
