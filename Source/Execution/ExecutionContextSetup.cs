/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Threading.Tasks;
using DolittleOptions = Dolittle.AspNetCore.Configuration.Options;
using Dolittle.Execution;
using Dolittle.Tenancy;
using Dolittle.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Dolittle.Logging;
using System.Security.Claims;

namespace Dolittle.AspNetCore.Execution
{
    /// <summary>
    /// Provides an endpoint for that sets the <see cref="Dolittle.Tenancy.TenantId"/> of the <see cref="Dolittle.Execution.ExecutionContext" />
    /// </summary>
    public class ExecutionContextSetup
    {
        readonly RequestDelegate _next;
        readonly IOptionsMonitor<DolittleOptions> _options;
        readonly IExecutionContextManager _executionContextManager;
        readonly ILogger _logger;

        /// <summary>
        /// Instantiates an instance of <see cref="ExecutionContextSetup"/>
        /// </summary>
        /// <param name="next"></param>
        /// <param name="options"></param>
        /// <param name="executionContextManager"></param>
        /// <param name="logger"></param>
        public ExecutionContextSetup(
            RequestDelegate next,
            IOptionsMonitor<DolittleOptions> options,
            IExecutionContextManager executionContextManager,
            ILogger logger
        )
        {
            _next = next;
            _options = options;
            _executionContextManager = executionContextManager;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var tenantIdHeader = _options.CurrentValue.ExecutionContextSetup.TenantIdHeaderName;
            var tenantId = GetTenantIdFromHeaders(context, tenantIdHeader);

            var shouldSkipAuthentication = _options.CurrentValue.ExecutionContextSetup.SkipAuthentication;
            if (!shouldSkipAuthentication && TryGetAuthenticatedUser(await context.AuthenticateAsync(), out var principal))
            {
                _executionContextManager.CurrentFor(tenantId, CorrelationId.New(), principal.ToClaims());
            }
            else
            {
                _executionContextManager.CurrentFor(tenantId, CorrelationId.New());
            }
            await _next.Invoke(context);
        }

        TenantId GetTenantIdFromHeaders(HttpContext context, string header)
        {
            var values = context.Request.Headers[header];
            ThrowIfTenantIdHeaderHasMultipleValues(header, values);
            if (values.Count == 1)
            {
                if (Guid.TryParse(values.First(), out var tenantId))
                {
                    return tenantId;
                }
                else
                {
                    _logger.Error($"The configured TenantId header '{header}' must be a valud Guid. The value was '{values.First()}' - no tenant will be configurd");
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