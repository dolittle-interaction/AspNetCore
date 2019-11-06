/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Dolittle.AspNetCore.Authentication
{

    /// <summary>
    /// The exception that gets thrown when there is a claim header on the http request with multiple values
    /// </summary>
    [Serializable]
    public class ClaimHasMultipleValues : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimHasMultipleValues"/>
        /// </summary>
        /// <param name="claim">The name of the claim</param>
        public ClaimHasMultipleValues(string claim)
            : base(claim)
        {}


    }
}