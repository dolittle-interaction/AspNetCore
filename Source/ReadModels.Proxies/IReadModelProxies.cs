/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 doLittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Linq;
using System.Text;
using doLittle.CodeGeneration;
using doLittle.CodeGeneration.JavaScript;
using doLittle.Execution;
using doLittle.Strings;
using doLittle.ReadModels;
using doLittle.Types;

namespace doLittle.AspNetCore.ReadModels.Proxies
{

    /// <summary>
    /// 
    /// </summary>
    public interface IReadModelProxies
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string Generate();
    }
}
