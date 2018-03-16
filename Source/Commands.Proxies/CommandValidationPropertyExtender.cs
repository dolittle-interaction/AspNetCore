/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.CodeGeneration.JavaScript;
using Dolittle.Validation.MetaData;
using Newtonsoft.Json;

namespace Dolittle.AspNetCore.Commands.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandValidationPropertyExtender : ICanExtendCommandProperty
    {
        IValidationMetaData _validationMetaData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationMetaData"></param>
        public CommandValidationPropertyExtender(IValidationMetaData validationMetaData)
        {
            _validationMetaData = validationMetaData;
        }

        /// <inheritdoc/>
        public void Extend(Type commandType, string propertyName, Observable observable)
        {
            var metaData = _validationMetaData.GetMetaDataFor(commandType);
            if (metaData.Properties.ContainsKey(propertyName))
            {
                var options = JsonConvert.SerializeObject(metaData.Properties[propertyName],
                    new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
                observable.ExtendWith("validation", options);
            }
        }
    }
}
