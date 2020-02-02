// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Dolittle.Types;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dolittle.AspNetCore.Debugging.Swagger.SwaggerGen
{
    /// <summary>
    /// Dolittle overload of <see cref="Swashbuckle.AspNetCore.SwaggerGen.SchemaRegistry" />.
    /// </summary>
    public class SchemaRegistry : ISchemaRegistry
    {
        readonly ISchemaRegistry _originalRegistry;
        readonly IInstancesOf<ICanProvideSwaggerSchemas> _providers;
        readonly SchemaIdManager _idManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaRegistry"/> class.
        /// </summary>
        /// <param name="originalRegistry"><see cref="ISchemaRegistry"/> for holding schemas.</param>
        /// <param name="providers"><see cref="IInstancesOf{T}"/> of <see cref="ICanProvideSwaggerSchemas"/>.</param>
        /// <param name="idManager"><see cref="SchemaIdManager"/> for managing schemaa identifiers.</param>
        public SchemaRegistry(
            ISchemaRegistry originalRegistry,
            IInstancesOf<ICanProvideSwaggerSchemas> providers,
            SchemaIdManager idManager)
        {
            _originalRegistry = originalRegistry;
            _providers = providers;
            _idManager = idManager;
        }

        /// <inheritdoc/>
        public IDictionary<string, Schema> Definitions => _originalRegistry.Definitions;

        /// <inheritdoc/>
        public Schema GetOrRegister(Type type)
        {
            foreach (var provider in _providers)
            {
                if (provider.CanProvideFor(type))
                {
                    return provider.ProvideFor(type, this, _idManager);
                }
            }

            return _originalRegistry.GetOrRegister(type);
        }
    }
}