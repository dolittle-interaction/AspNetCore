// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Dolittle.Queries;

namespace Debugging.MyFeature
{
    /// <summary>
    /// Represents a query for testing.
    /// </summary>
    public class MyQuery : IQueryFor<MyReadModel>
    {
        /// <summary>
        /// Gets the actual query.
        /// </summary>
        public IQueryable<MyReadModel> Query => new MyReadModel[] { new MyReadModel() }.AsQueryable();
    }
}