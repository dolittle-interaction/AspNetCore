/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Dolittle.Collections;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dolittle.AspNetCore.Authentication
{

    /// <summary>
    /// Represents an <see cref="AuthenticationHandler{Scheme}"/> for handling authentication
    /// </summary>
    public class Handler : AuthenticationHandler<SchemeOptions>
    {
        /// <summary>
        /// Instantiates an instance of <see cref="Handler"/>
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        /// <returns></returns>
        public Handler(IOptionsMonitor<SchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock) {}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>();
            if (Request.Headers.TryGetValue("Claims", out var claimsValues)) {

                if (!claimsValues.Any()) return Task.FromResult(AuthenticateResult.NoResult());
                claimsValues.ForEach(_ => {
                    var split = _.Split('=', 2);
                    claims.Add(new Claim(split[0], split[1]));
                });
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(
                    new ClaimsPrincipal(new []{new ClaimsIdentity(claims, "Dolittle.Headers")}),
                    new AuthenticationProperties(),
                    Scheme.Name
                )));
            }
            else 
                return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}