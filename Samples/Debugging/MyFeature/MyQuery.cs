// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Dolittle.Queries;

namespace Debugging.MyFeature
{
    /// <summary>
    /// lkm.
    /// </summary>
    public class MyQuery : IQueryFor<MyReadModel>
    {
        /// <summary>
        /// gets lmo.
        /// </summary>
        public IQueryable<MyReadModel> Query { get; }
    }
}