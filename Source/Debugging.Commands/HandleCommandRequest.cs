// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Artifacts;
using Dolittle.PropertyBags;
using Dolittle.Tenancy;

namespace Dolittle.AspNetCore.Debugging.Commands
{
    /// <summary>
    /// Captures the information necessary to call the command coordinator.
    /// </summary>
    public class HandleCommandRequest
    {
        /// <summary>
        /// Gets or sets the tenant to handle the command in.
        /// </summary>
        public TenantId Tenant { get; set; }

        /// <summary>
        /// Gets or sets the artifact identifying the command.
        /// </summary>
        public Artifact Artifact { get; set; }

        /// <summary>
        /// Gets or sets the actual command data.
        /// </summary>
        public PropertyBag Command { get; set; }
    }
}