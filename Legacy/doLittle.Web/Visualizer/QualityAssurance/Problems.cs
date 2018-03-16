/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Dolittle.Diagnostics;
using Dolittle.Read;

namespace Dolittle.Web.Visualizer.QualityAssurance
{
    public class Problems : IReadModel
    {
        public IEnumerable<Problem> All { get; set; }
    }
}
