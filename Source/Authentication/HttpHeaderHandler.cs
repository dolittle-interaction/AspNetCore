/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
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
    public class HttpHeaderHandler : AuthenticationHandler<HttpHeaderSchemeOptions>
    {
        /// <summary>
        /// Instantiates an instance of <see cref="HttpHeaderHandler"/>
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        /// <returns></returns>
        public HttpHeaderHandler(IOptionsMonitor<HttpHeaderSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock) {}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>();
            var claimHeaders = Request.Headers.Where(_ => _.Key.StartsWith("Claim-"));
            if (claimHeaders.Any()) 
            {
                foreach (var header in claimHeaders)
                {
                    if (!header.Value.Any()) throw new ClaimHasNoValues(header.Key);
                    else if (header.Value.Count > 1) throw new ClaimHasMultipleValues(header.Key);
                    else claims.Add(new Claim(header.Key, header.Value.FirstOrDefault()));
                }
                var identities = new []{new ClaimsIdentity(claims, "Dolittle.Headers")};
                var principal = new ClaimsPrincipal(identities);
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(
                    principal,
                    new AuthenticationProperties(),
                    Scheme.Name
                )));
            }
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
