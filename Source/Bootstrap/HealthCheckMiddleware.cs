/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Dolittle.AspNetCore.Bootstrap
{
    /// <summary>
    /// Provides an endpoint for monitoring health from the outside.
    /// </summary>
    public class HealthCheckMiddleware
    {
        readonly RequestDelegate _next;
        readonly PathString Path = new PathString("/dolittle/healthcheck");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public HealthCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        { 
            if (!context.Request.Path.Equals(Path))
            {
                await _next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await _next.Invoke(context);
            }
        }
    }
}