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

namespace Dolittle.AspNetCore.Bootstrap
{
    /// <summary>
    /// Provides an endpoint for that sets the <see cref="Dolittle.Tenancy.TenantId"/> of the <see cref="Dolittle.Execution.ExecutionContext" />
    /// </summary>
    public class ExecutionContextSetup
    {
        readonly RequestDelegate _next;
        readonly IExecutionContextManager _executionContextManager;
        readonly ExecutionContextSetupConfigurationDelegate _callback;
        readonly ExecutionContextSetupConfiguration _configuration;
        
        /// <summary>
        /// Instantiates an instance of <see cref="ExecutionContextSetup"/>
        /// </summary>
        /// <param name="next"></param>
        /// <param name="executionContextManager"></param>
        /// <param name="executionContextSetupConfigurationCallback"></param>
        public ExecutionContextSetup(RequestDelegate next, IExecutionContextManager executionContextManager, ExecutionContextSetupConfigurationDelegate executionContextSetupConfigurationCallback)
        {
            _next = next;
            _executionContextManager = executionContextManager;
            _callback = executionContextSetupConfigurationCallback ?? ExecutionContextSetupConfiguration.DefaultDelegate;
            _configuration = _callback(ExecutionContextSetupConfiguration.Default);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var tenantId = TenantId.Unknown;
            var tenantIdHeaderValue = context.Request.Headers[_configuration.TenantIdHeaderKey].FirstOrDefault();
            if (!string.IsNullOrEmpty(tenantIdHeaderValue)) 
            {
                try 
                {
                    tenantId = new TenantId{Value = Guid.Parse(tenantIdHeaderValue)};
                } 
                catch {}
            }
            _executionContextManager.CurrentFor(tenantId, CorrelationId.New(), context.User.ToClaims());
            await _next.Invoke(context);
        }
    }
}