// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dolittle.AspNetCore.Debugging.Handlers;
using Dolittle.Queries;
using Dolittle.Queries.Coordination;
using Microsoft.AspNetCore.Http;

namespace Dolittle.AspNetCore.Debugging.Artifacts.Queries
{
    /// <summary>
    /// hi ther.
    /// </summary>
    public class DebuggingHandler : IDebuggingHandler, ICanHandlePostRequests<IQuery>
    {
        readonly IArtifactMapper<IQuery> _queries;
        readonly IQueryCoordinator _queryCoordinator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggingHandler"/> class.
        /// </summary>
        /// <param name="queries">me the qwuer.</param>
        /// <param name="queryCoordinator">me thq qvuer cord.</param>
        public DebuggingHandler(IArtifactMapper<IQuery> queries, IQueryCoordinator queryCoordinator)
        {
            _queries = queries;
            _queryCoordinator = queryCoordinator;

            foreach (var query in _queries.Artifacts)
            {
                Artifacts.Add(_queries.GetPathFor(query), query);
            }
        }

        /// <inheritdoc/>
        public string Name => "Queries";

        /// <inheritdoc/>
        public string Title => "Query Queries";

        /// <inheritdoc/>
        public IDictionary<PathString, Type> Artifacts { get; } = new Dictionary<PathString, Type>();

        /// <inheritdoc/>
        public IDictionary<int, string> Responses => new Dictionary<int, string>
        {
            { StatusCodes.Status200OK, "The query uhhh ecxists i guess." },
            { StatusCodes.Status500InternalServerError, "This query ymmm its no good." },
        };

        /// <inheritdoc/>
        public async Task HandlePostRequest(HttpContext context, IQuery artifact)
        {
            var paging = new PagingInfo();
            var queryResult = await _queryCoordinator.Execute(artifact, paging).ConfigureAwait(false);
            if (queryResult.Success)
            {
                await context.RespondWithOk($"Query {artifact.GetType()} executed successfully. \n{queryResult}").ConfigureAwait(false);
            }
            else
            {
                await context.RespondWithError($"Query {artifact.GetType()} wasn't executed correctly. \n{queryResult.Exception}").ConfigureAwait(false);
            }
        }
    }
}
