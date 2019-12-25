// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
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
    /// Represents an <see cref="AuthenticationHandler{Scheme}"/> for handling authentication.
    /// </summary>
    public class HttpHeaderHandler : AuthenticationHandler<HttpHeaderSchemeOptions>
    {
        readonly IOptionsMonitor<HttpHeaderSchemeOptions> _options;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHeaderHandler"/> class.
        /// </summary>
        /// <param name="options">Options for HttpHeader scheme.</param>
        /// <param name="loggerFactory"><see cref="ILoggerFactory"/> for logging.</param>
        /// <param name="encoder"><see cref="UrlEncoder"/> to use.</param>
        /// <param name="clock">The <see cref="ISystemClock"/>.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public HttpHeaderHandler(
            IOptionsMonitor<HttpHeaderSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock,
            ILogger logger)
            : base(options, loggerFactory, encoder, clock)
        {
            _options = options;
            _logger = logger;
        }

        /// <inheritdoc/>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claimHeaderName = _options.CurrentValue.ClaimsHeaderName;
            var claimHeaders = Request.Headers[claimHeaderName];
            if (claimHeaders.Count > 0)
            {
                var mergedClaimHeaders = string.Join(',', claimHeaders);
                var identity = ParseClaimHeaders(mergedClaimHeaders);
                if (ValidateClaimsIdentity(identity))
                {
                    return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(
                        new ClaimsPrincipal(identity),
                        new AuthenticationProperties(),
                        Scheme.Name)));
                }
                else
                {
                    _logger.Warning($"There were '{claimHeaderName}' headers present on the request. But the resulting identity was not valid. No authenticated user will be set.");
                }
            }

            return Task.FromResult(AuthenticateResult.NoResult());
        }

        ClaimsIdentity ParseClaimHeaders(string claimsHeaderValue)
        {
            var identity = new ClaimsIdentity("Dolittle.Headers");
            foreach (var claimTypeValue in claimsHeaderValue.Split(','))
            {
                if (!string.IsNullOrWhiteSpace(claimTypeValue) && claimTypeValue.Contains('=', StringComparison.InvariantCulture))
                {
                    var splitClaimTypeValue = claimTypeValue.Split('=', 2);
                    identity.AddClaim(new Claim(splitClaimTypeValue[0], splitClaimTypeValue[1]));
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

            if (!claims.HasClaim(_ => _.Type == "iss"))
            {
                _logger.Error("Provided Claim headers does not contain the required 'iss' claim");
                return false;
            }

            return true;
        }
    }
}
