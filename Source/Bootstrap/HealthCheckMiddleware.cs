// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
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
        readonly PathString _path = new PathString("/dolittle/healthcheck");

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next middleware.</param>
        public HealthCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Middleware invocation method.
        /// </summary>
        /// <param name="context">Current <see cref="HttpContext"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.Equals(_path, StringComparison.InvariantCulture))
            {
                await _next.Invoke(context).ConfigureAwait(false);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await _next.Invoke(context).ConfigureAwait(false);
            }
        }
    }
}