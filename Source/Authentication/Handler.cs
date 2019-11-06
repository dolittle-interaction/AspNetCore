/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dolittle.AspNetCore.Authentication
{

    /// <summary>
    /// Represents an <see cref="AuthenticationHandler{Scheme}"/> for handling authentication
    /// </summary>
    public class Handler : AuthenticationHandler<Scheme>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            
            return AuthenticateResult.Success(new AuthenticationTicket(
                new ClaimsPrincipal(),
                new AuthenticationProperties(),
                Scheme.Name
            ));
        }
    }
}