/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Threading.Tasks;
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
        readonly ExecutionContextSetupConfiguration _executionContextSetupConfiguration;
        readonly ExecutionContextSetupConfigurationDelegate _callback;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="executionContextSetupConfigurationCallback"></param>
        public ExecutionContextSetup(RequestDelegate next, ExecutionContextSetupConfigurationDelegate executionContextSetupConfigurationCallback)
        {
            _next = next;
            _callback = executionContextSetupConfigurationCallback ?? ExecutionContextSetupConfiguration.DefaultDelegate;
            _executionContextSetupConfiguration = _callback(ExecutionContextSetupConfiguration.Default);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        { 
            
            await _next.Invoke(context);
        
        }
    }
}