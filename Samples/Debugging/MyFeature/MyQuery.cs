// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Dolittle.Queries;
using MongoDB.Driver;

namespace Debugging.MyFeature
{
    /// <summary>
    /// Example Query.
    /// </summary>
    public class MyQuery : IQueryFor<MyReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyQuery"/> class.
        /// </summary>
        /// <param name="collection">Collection of all the MyReadModel's.</param>
        public MyQuery(IMongoCollection<MyReadModel> collection) => Query = collection?.AsQueryable();

        /// <summary>
        /// Gets an empty Array.
        /// </summary>
        public IQueryable<MyReadModel> Query { get; }
    }
}
