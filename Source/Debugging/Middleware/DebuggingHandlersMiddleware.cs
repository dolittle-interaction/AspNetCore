// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dolittle.AspNetCore.Debugging.Handlers;
using Dolittle.Reflection;
using Dolittle.Serialization.Json;
using Dolittle.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Dolittle.AspNetCore.Debugging.Middleware
{
    /// <summary>
    /// Middleware that handles Swagger debugging operations for implemenations of <see cref="IDebuggingHandler"/>.
    /// </summary>
    public class DebuggingHandlersMiddleware
    {
        readonly RequestDelegate _next;
        readonly IOptions<DebuggingOptions> _options;
        readonly IInstancesOf<IDebuggingHandler> _handlers;
        readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggingHandlersMiddleware"/> class.
        /// </summary>
        /// <param name="next">The <see cref="RequestDelegate"/> used to call the next middleware.</param>
        /// <param name="options">The <see cref="DebuggingOptions"/> used to configure the middleware.</param>
        /// <param name="handlers">All implementations of <see cref="IDebuggingHandler"/>.</param>
        /// <param name="serializer">The <see cref="ISerializer"/> used to deserialize the artifact from the <see cref="HttpRequest"/> body.</param>
        public DebuggingHandlersMiddleware(
            RequestDelegate next,
            IOptions<DebuggingOptions> options,
            IInstancesOf<IDebuggingHandler> handlers,
            ISerializer serializer)
        {
            _next = next;
            _options = options;
            _handlers = handlers;
            _serializer = serializer;
        }

        /// <summary>
        /// Middleware invocation method.
        /// </summary>
        /// <param name="context">Current <see cref="HttpContext"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(_options.Value.BasePath, StringComparison.InvariantCultureIgnoreCase, out var debuggingPath))
            {
                foreach (var handler in _handlers)
                {
                    if (debuggingPath.StartsWithSegments($"/{handler.Name}", StringComparison.InvariantCultureIgnoreCase, out var handlerPath))
                    {
                        await InvokeDebuggingHandler(context, handler, handlerPath).ConfigureAwait(false);
                        return;
                    }
                }

                await context.RespondWithNotFound("Debuggin handler").ConfigureAwait(false);
                return;
            }

            await _next(context).ConfigureAwait(false);
        }

        async Task InvokeDebuggingHandler(HttpContext context, IDebuggingHandler handler, PathString path)
        {
            if (handler.Aritfacts.TryGetValue(path, out var artifactType))
            {
                var json = await ReadRequestBodyAsString(context.Request).ConfigureAwait(false);

                object artifact;
                try
                {
                    artifact = _serializer.FromJson(artifactType, json);
                }
                catch (Exception exception)
                {
                    await context.RespondWithException(exception).ConfigureAwait(false);
                    return;
                }

                if (HttpMethods.IsGet(context.Request.Method) && handler.GetType().ImplementsOpenGeneric(typeof(ICanHandleGetRequests<>)))
                {
                    await InvokeHandlerMethod(context, handler, typeof(ICanHandleGetRequests<>), artifact).ConfigureAwait(false);
                    return;
                }

                if (HttpMethods.IsPost(context.Request.Method) && handler.GetType().ImplementsOpenGeneric(typeof(ICanHandlePostRequests<>)))
                {
                    await InvokeHandlerMethod(context, handler, typeof(ICanHandlePostRequests<>), artifact).ConfigureAwait(false);
                    return;
                }

                await context.RespondWithBadRequest($"method {context.Request.Method} not supported").ConfigureAwait(false);
                return;
            }

            await context.RespondWithNotFound($"Artifact on path {path}").ConfigureAwait(false);
        }

        async Task InvokeHandlerMethod(HttpContext context, IDebuggingHandler handler, Type handlerType, object artifact)
        {
            var handlerInterface = handler.GetType().GetInterfaces().First(_ => _.IsGenericType && _.GetGenericTypeDefinition().GetTypeInfo() == handlerType.GetTypeInfo());
            var artifactBaseType = handlerInterface.GenericTypeArguments[0];

            if (!artifactBaseType.IsAssignableFrom(artifact.GetType()))
            {
                await context.RespondWithError($"Cannot assign artifact of type {artifact.GetType()} to handler method base type {artifactBaseType}.").ConfigureAwait(false);
                return;
            }

            var handlerMethod = handlerInterface.GetMethods().First(_ => _.Name.StartsWith("Handle", StringComparison.InvariantCultureIgnoreCase));
            var task = handlerMethod.Invoke(handler, new[] { context, artifact }) as Task;
            await task.ConfigureAwait(false);
        }

        async Task<string> ReadRequestBodyAsString(HttpRequest request)
        {
            ReadResult result;
            while (true)
            {
                result = await request.BodyReader.ReadAsync().ConfigureAwait(false);
                if (result.IsCompleted) break;
                request.BodyReader.AdvanceTo(result.Buffer.Start, result.Buffer.End);
            }

            var body = Encoding.UTF8.GetString(result.Buffer.ToArray());
            request.BodyReader.AdvanceTo(result.Buffer.End);
            return body;
        }
    }
}