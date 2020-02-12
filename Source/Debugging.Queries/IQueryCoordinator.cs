// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Queries;
using Dolittle.Tenancy;

namespace Dolittle.AspNetCore.Debugging.Queries
{
    /// <summary>
    /// Represents a coordinator capable of executing queries.
    /// </summary>
    public interface IQueryCoordinator
    {
        /// <summary>
        /// Execute a query.
        /// </summary>
        /// <param name="tenant">The <see cref="TenantId"/> to execute in context of.</param>
        /// <param name="query">The instance of the <see cref="IQuery"/> to execute.</param>
        /// <returns>The <see cref="QueryResult"/> from executing the query.</returns>
        QueryResult Execute(TenantId tenant, IQuery query);
    }
}