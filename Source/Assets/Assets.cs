/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Dolittle.AspNetCore.Assets
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/Dolittle/Assets")]
    public class Assets : ControllerBase
    {
        readonly IAssetsManager _assetsManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetsManager"></param>
        public Assets(IAssetsManager assetsManager)
        {
            _assetsManager = assetsManager;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> GetFor([FromQuery] string extension)
        {
            return _assetsManager.GetFilesForExtension(extension);
        }
    }
}