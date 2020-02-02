// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Artifacts;
using Dolittle.PropertyBags;
using Dolittle.Tenancy;

namespace Dolittle.AspNetCore.Debugging.Queries
{
    /// <summary>
    /// Captures the information necessary to call the query coordinator.
    /// </summary>
    public class ExecuteQueryRequest
    {
        /// <summary>
        /// Gets or sets the tenant to handle the query in.
        /// </summary>
        public TenantId Tenant { get; set; }

        /// <summary>
        /// Gets or sets the artifact identifying the query.
        /// </summary>
        public Artifact Artifact { get; set; }

        /// <summary>
        /// Gets or sets the actual query data.
        /// </summary>
        public PropertyBag Query { get; set; }
    }
}