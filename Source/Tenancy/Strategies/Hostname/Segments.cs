/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
namespace Dolittle.Tenancy.Strategies.Hostname
{
    /// <summary>
    /// An enum representing how the <see cref="TenantId"/> should be retrieved from the http request hostname
    /// </summary>
    public enum Segments
    {
        /// <summary>
        /// Get <see cref="TenantId"/> using all segments for the mapping
        /// </summary>
        All,
        /// <summary>
        /// Get <see cref="TenantId"/> using the first segment for the mapping
        /// </summary>
        First
    }
}