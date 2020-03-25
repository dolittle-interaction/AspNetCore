// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Dolittle.Execution;
using Dolittle.Logging;
using Dolittle.Security;
using Dolittle.Tenancy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using DolittleOptions = Dolittle.AspNetCore.Configuration.Options;

namespace Dolittle.AspNetCore.Execution
{
    /// <summary>
    /// Provides an endpoint for that sets the <see cref="TenantId"/> of the <see cref="ExecutionContext" />.
    /// </summary>
    public class ExecutionContextSetup
    {
        readonly RequestDelegate _next;
        readonly IOptionsMonitor<DolittleOptions> _options;
        readonly IExecutionContextManager _executionContextManager;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionContextSetup"/> class.
        /// </summary>
        /// <param name="next">Next middleware.</param>
        /// <param name="options">Configuration in form of <see cref="DolittleOptions"/>.</param>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with <see cref="ExecutionContext"/>.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public ExecutionContextSetup(
            RequestDelegate next,
            IOptionsMonitor<DolittleOptions> options,
            IExecutionContextManager executionContextManager,
            ILogger logger)
        {
            _next = next;
            _options = options;
            _executionContextManager = executionContextManager;
            _logger = logger;
        }

        /// <summary>
        /// Middleware invocation method.
        /// </summary>
        /// <param name="context">Current <see cref="HttpContext"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var tenantIdHeader = _options.CurrentValue.ExecutionContextSetup.TenantIdHeaderName;
            var tenantId = GetTenantIdFromHeaders(context, tenantIdHeader);

            var shouldSkipAuthentication = _options.CurrentValue.ExecutionContextSetup.SkipAuthentication;
            if (!shouldSkipAuthentication && TryGetAuthenticatedUser(await context.AuthenticateAsync().ConfigureAwait(false), out var principal))
            {
                _executionContextManager.CurrentFor(tenantId, CorrelationId.New(), principal.ToClaims());
            }
            else
            {
                _executionContextManager.CurrentFor(tenantId, CorrelationId.New());
            }

            await _next.Invoke(context).ConfigureAwait(false);
        }

        TenantId GetTenantIdFromHeaders(HttpContext context, string header)
        {
            var values = context.Request.Headers[header];
            ThrowIfTenantIdHeaderHasMultipleValues(header, values);
            if (values.Count == 1)
            {
                if (Guid.TryParse(values[0], out var tenantId))
                {
                    return tenantId;
                }
                else
                {
                    _logger.Error($"The configured TenantId header '{header}' must be a valid Guid. The value was '{values[0]}' - no tenant will be configured");
                }
            }

            return TenantId.Unknown;
        }

        void ThrowIfTenantIdHeaderHasMultipleValues(string header, StringValues values)
        {
            if (values.Count > 1)
            {
                throw new TenantIdHeaderHasMultipleValues(header);
            }
        }

        bool TryGetAuthenticatedUser(AuthenticateResult authenticateResult, out ClaimsPrincipal principal)
        {
            principal = authenticateResult.Principal;
            return authenticateResult.Succeeded;
        }
    }
}