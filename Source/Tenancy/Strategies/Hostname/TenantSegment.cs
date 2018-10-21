/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Concepts;

namespace Dolittle.Tenancy.Strategies.Hostname
{
    /// <summary>
    /// Represents the concept of a tenant segment of a hostname
    /// </summary>
    public class TenantSegment : ConceptAs<string>
    {

        /// <summary>
        /// Implicitly convert from <see cref="string"/> to <see cref="TenantSegment"/>
        /// </summary>
        /// <param name="segment"></param>
        public static implicit operator TenantSegment(string segment)
        {
            return new TenantSegment { Value = segment };
        }
    }
}
