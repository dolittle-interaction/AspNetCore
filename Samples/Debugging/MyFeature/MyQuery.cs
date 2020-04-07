// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Dolittle.Queries;

namespace Debugging.MyFeature
{
    /// <summary>
    /// Example Query.
    /// </summary>
    public class MyQuery : IQueryFor<MyReadModel>
    {
        /// <summary>
        /// Gets an empty Array.
        /// </summary>
        public IQueryable<MyReadModel> Query => Array.Empty<MyReadModel>().AsQueryable();
    }
}