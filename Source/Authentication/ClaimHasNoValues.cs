/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
namespace Dolittle.AspNetCore.Authentication
{

    /// <summary>
    /// The exception that gets thrown when there is a claim header on the http request without any values
    /// </summary>
    [Serializable]
    public class ClaimHasNoValues : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimHasNoValues"/>
        /// </summary>
        /// <param name="claim">The name of the claim</param>
        public ClaimHasNoValues(string claim)
            : base(claim)
        {}


    }
}