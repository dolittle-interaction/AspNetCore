/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ILogger = Dolittle.Logging.ILogger;

namespace Dolittle.AspNetCore.Authentication
{

    /// <summary>
    /// Represents an <see cref="AuthenticationHandler{Scheme}"/> for handling authentication
    /// </summary>
    public class HttpHeaderHandler : AuthenticationHandler<HttpHeaderSchemeOptions>
    {
        readonly ILogger _logger;

        /// <summary>
        /// Instantiates an instance of <see cref="HttpHeaderHandler"/>
        /// </summary>
        /// <param name="options"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public HttpHeaderHandler(IOptionsMonitor<HttpHeaderSchemeOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder, ISystemClock clock, ILogger logger) : base(options, loggerFactory, encoder, clock)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claimHeaders = Request.Headers["Claim"];
            if (claimHeaders.Any())
            {
                var identity = ParseClaimHeaders(claimHeaders);
                if (ValidateClaimsIdentity(identity))
                {
                    return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(
                        new ClaimsPrincipal(identity),
                        new AuthenticationProperties(),
                        Scheme.Name
                    )));
                }
            }
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        ClaimsIdentity ParseClaimHeaders(IEnumerable<string> claimHeaders)
        {
            var identity = new ClaimsIdentity("Dolittle.Headers");
            foreach (var claimHeader in claimHeaders)
            {
                if (!string.IsNullOrWhiteSpace(claimHeader) && claimHeader.Contains('-'))
                {
                    var claimTypeValue = claimHeader.Split('-',2);
                    identity.AddClaim(new Claim(claimTypeValue[0], claimTypeValue[1]));
                }
            }
            return identity;
        }

        bool ValidateClaimsIdentity(ClaimsIdentity claims)
        {
            if (!claims.HasClaim(_ => _.Type == "sub"))
            {
                _logger.Error("Provided Claim headers does not contain the required 'sub' claim");
                return false;
            }
            return true;
        }
    }
}
