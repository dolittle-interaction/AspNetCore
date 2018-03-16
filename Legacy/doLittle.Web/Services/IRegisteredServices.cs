/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace Dolittle.Web.Services
{
    public interface IRegisteredServices
    {
        IEnumerable<KeyValuePair<Type, string>> GetAll();
        void Register(Type service, string url);
    }
}