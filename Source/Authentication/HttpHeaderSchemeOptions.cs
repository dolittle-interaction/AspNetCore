// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Authentication;

namespace Dolittle.AspNetCore.Authentication
{
    /// <summary>
    /// Represents the <see cref="AuthenticationSchemeOptions"/> for <see cref="HttpHeaderHandler"/> .
    /// </summary>
    public class HttpHeaderSchemeOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// Gets or sets the name of the HTTP Header that contains the Claims.
        /// </summary>
        public string ClaimsHeaderName { get; set; } = "Claims";
    }
}