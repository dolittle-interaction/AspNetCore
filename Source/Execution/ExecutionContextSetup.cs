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
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

namespace Dolittle.AspNetCore.Bootstrap
{
    /// <summary>
    /// Provides an endpoint for that sets the <see cref="Dolittle.Tenancy.TenantId"/> of the <see cref="Dolittle.Execution.ExecutionContext" />
    /// </summary>
    public class ExecutionContextSetup
    {
        readonly RequestDelegate _next;
        readonly IExecutionContextManager _executionContextManager;
        readonly ExecutionContextSetupConfiguration _configuration;
        
        /// <summary>
        /// Instantiates an instance of <see cref="ExecutionContextSetup"/>
        /// </summary>
        /// <param name="next"></param>
        /// <param name="executionContextManager"></param>
        public ExecutionContextSetup(RequestDelegate next, IExecutionContextManager executionContextManager)
        {
            _next = next;
            _executionContextManager = executionContextManager;
            _configuration = ExecutionContextSetupConfiguration.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var authResult = await context.AuthenticateAsync("Dolittle.Headers");
            var tenantId = TenantId.Unknown;
            
            var tenantIdHeaders = context.Request.Headers[_configuration.TenantIdHeaderKey];

            if (tenantIdHeaders.Count == 1) 
            {
                var tenantIdValue = tenantIdHeaders.FirstOrDefault();
                if (Guid.TryParse(tenantIdValue, out var tenantIdGuid))
                    tenantId = new TenantId{Value = tenantIdGuid};
            }
            if (authResult.Succeeded)
                _executionContextManager.CurrentFor(tenantId, CorrelationId.New(), authResult.Principal.ToClaims());
            else 
                _executionContextManager.CurrentFor(tenantId, CorrelationId.New());
            await _next.Invoke(context);
        }
    }
}