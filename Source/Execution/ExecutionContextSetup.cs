/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Threading.Tasks;
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
        readonly IOptionsMonitor<ExecutionContextSetupConfiguration> _configuration;
        readonly IExecutionContextManager _executionContextManager;
        
        /// <summary>
        /// Instantiates an instance of <see cref="ExecutionContextSetup"/>
        /// </summary>
        /// <param name="next"></param>
        /// <param name="configuration"></param>
        /// <param name="executionContextManager"></param>
        public ExecutionContextSetup(RequestDelegate next, IOptionsMonitor<ExecutionContextSetupConfiguration> configuration, IExecutionContextManager executionContextManager)
        {
            _next = next;
            _configuration = configuration;
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
            
            var headerName = _configuration.CurrentValue.TenantIdHeaderName;
            var tenantIdHeaders = context.Request.Headers[headerName];

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