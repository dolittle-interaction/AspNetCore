/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.AspNetCore.Mvc;

namespace Dolittle.AspNetCore.Bootstrap
{
    /// <summary>
    /// Defines something that can add to <see cref="MvcOptions"/>
    /// </summary>
    public interface ICanAddMvcOptions
    {
        /// <summary>
        /// Add options to <see cref="MvcOptions"/>
        /// </summary>
        /// <param name="options"><see cref="MvcOptions"/> to add to</param>
        void Add(MvcOptions options);
    }
}