// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.DependencyInversion;
using Dolittle.Execution;
using Dolittle.Queries;
using Dolittle.Tenancy;
using IRuntimeQueryCoordinator = Dolittle.Queries.Coordination.IQueryCoordinator;

namespace Dolittle.AspNetCore.Debugging.Queries
{
    /// <summary>
    /// An implementation of <see cref="IQueryCoordinator"/>.
    /// </summary>
    public class QueryCoordinator : IQueryCoordinator
    {
        readonly IExecutionContextManager _executionContextManager;
        readonly IRuntimeQueryCoordinator _runtimeQueryCoordinator;
        readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryCoordinator"/> class.
        /// </summary>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with <see cref="ExecutionContext"/>.</param>
        /// <param name="runtimeQueryCoordinator"><see cref="IRuntimeQueryCoordinator"/> for coordinating queries.</param>
        /// <param name="container"><see cref="IContainer"/> for getting instances of queries.</param>
        public QueryCoordinator(
            IExecutionContextManager executionContextManager,
            IRuntimeQueryCoordinator runtimeQueryCoordinator,
            IContainer container)
        {
            _executionContextManager = executionContextManager;
            _runtimeQueryCoordinator = runtimeQueryCoordinator;
            _container = container;
        }

        /// <inheritdoc/>
        public QueryResult Execute(TenantId tenant, IQuery query)
        {
            _executionContextManager.CurrentFor(tenant);
            var instance = _container.Get(query.GetType()) as IQuery;

            foreach (var property in query.GetType().GetProperties())
            {
                if (!property.Name.Equals("Query", StringComparison.InvariantCulture))
                {
                    property.SetValue(instance, property.GetValue(query));
                }
            }

            return _runtimeQueryCoordinator.Execute(instance, new PagingInfo { Number = 0, Size = int.MaxValue }).Result;
        }
    }
}