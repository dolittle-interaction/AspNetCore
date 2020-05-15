// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.AspNetCore.Debugging.Handlers;
using Dolittle.AspNetCore.Generators.Documents;
using Dolittle.AspNetCore.Generators.Schemas;
using Dolittle.DependencyInversion;
using Dolittle.Events;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dolittle.AspNetCore.Debugging.Artifacts.Events
{
    /// <summary>
    /// An implementation of <see cref="ICanModifyDebugginHandlerDocument"/> that adds the <see cref="EventSourceId"/> parameter to commit event operations.
    /// </summary>
    public class AddEventSourceIdToEventOperations : ICanModifyDebugginHandlerDocument
    {
        readonly FactoryFor<ISchemaGenerator> _schemaGeneratorFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEventSourceIdToEventOperations"/> class.
        /// </summary>
        /// <param name="schemaGeneratorFactory">A factory for <see cref="ISchemaGenerator"/>.</param>
        public AddEventSourceIdToEventOperations(FactoryFor<ISchemaGenerator> schemaGeneratorFactory)
        {
            _schemaGeneratorFactory = schemaGeneratorFactory;
        }

        /// <inheritdoc/>
        public void ModifyDocument(IDebuggingHandler handler, OpenApiDocument document)
        {
            if (handler is DebuggingHandler)
            {
                var schemaGenerator = _schemaGeneratorFactory();
                var repository = new SchemaRepository();
                repository.PopulateWithDocumentSchemas(document);

                foreach ((_, var item) in document.Paths)
                {
                    item.Parameters.Add(GenerateEventSourceIdParameter(schemaGenerator, repository));
                }

                document.Components.Schemas = repository.Schemas;
            }
        }

        OpenApiParameter GenerateEventSourceIdParameter(ISchemaGenerator schemaGenerator, SchemaRepository repository)
        {
            return new OpenApiParameter
            {
                Name = "EventSource-ID",
                In = ParameterLocation.Query,
                Required = true,
                Schema = schemaGenerator.GenerateSchema(typeof(EventSourceId), repository),
            };
        }
    }
}