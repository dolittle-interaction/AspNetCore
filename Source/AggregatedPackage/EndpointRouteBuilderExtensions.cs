// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Provides extension methods for <see cref="IEndpointRouteBuilder" />.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Adds a the Dolittle Application Model <see cref="RouteEndpoint">endpoints</see> to the <see cref="IEndpointRouteBuilder"/>.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <returns>A <see cref="IEndpointConventionBuilder"/> that can be used to further customize the endpoint.</returns>
        public static IEndpointRouteBuilder MapDolittleApplicationModel(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapDolittleCommandCoordinator();
            endpoints.MapDolittleQueryCoordinator();
            return endpoints;
        }
    }
}