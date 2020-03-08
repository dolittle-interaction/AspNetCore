// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Dolittle.AspNetCore.Debugging.Handlers
{
    /// <summary>
    /// <see cref="HttpContext"/> extensions for responding with text.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Responds with HTTP status 200 and text.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> that holds the response.</param>
        /// <param name="text">The text to respond with.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task RespondWithOk(this HttpContext context, string text)
        {
            await RespondWithStatusCodeAndText(context, StatusCodes.Status200OK, text).ConfigureAwait(false);
        }

        static async Task RespondWithStatusCodeAndText(HttpContext context, int statusCode, string text)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(text).ConfigureAwait(false);
        }
    }
}