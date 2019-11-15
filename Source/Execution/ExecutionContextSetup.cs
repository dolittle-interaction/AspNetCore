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
        
        /// <summary>
        /// Instantiates an instance of <see cref="ExecutionContextSetup"/>
        /// </summary>
        /// <param name="next"></param>
        /// <param name="options"></param>
        /// <param name="executionContextManager"></param>
        public ExecutionContextSetup(RequestDelegate next, IOptionsMonitor<DolittleOptions> options, IExecutionContextManager executionContextManager)
        {
            _next = next;
            _options = options;
            _executionContextManager = executionContextManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var tenantId = TenantId.Unknown;
            var authResult = await context.AuthenticateAsync();
            
            var headerName = _options.CurrentValue.ExecutionContextSetup.TenantIdHeaderName;
            var tenantIdHeaders = context.Request.Headers[headerName];

            // Tenant-ID
            // Portal-Owner-ID

            if (tenantIdHeaders.Count > 1) throw new TenantIdHeaderHasMultipleValues(headerName);
            else if (tenantIdHeaders.Count == 1)
            {
                var tenantIdString = tenantIdHeaders.FirstOrDefault();
                if (Guid.TryParse(tenantIdString, out var tenantIdGuid))
                {
                    tenantId = new TenantId{ Value = tenantIdGuid };
                }
                else
                {
                    throw new TenantIdHeaderHasInvalidTenantId(headerName);
                }
            }
            if (authResult.Succeeded)
                _executionContextManager.CurrentFor(tenantId, CorrelationId.New(), authResult.Principal.ToClaims());
            else 
                _executionContextManager.CurrentFor(tenantId, CorrelationId.New());
            await _next.Invoke(context);
        }
    }
}