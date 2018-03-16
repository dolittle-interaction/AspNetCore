/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Linq;
using System.Text;
using Dolittle.CodeGeneration;
using Dolittle.CodeGeneration.JavaScript;
using Dolittle.Execution;
using Dolittle.Strings;
using Dolittle.ReadModels;
using Dolittle.Types;

namespace Dolittle.AspNetCore.ReadModels.Proxies
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
