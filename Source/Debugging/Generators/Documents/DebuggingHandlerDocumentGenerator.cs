// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Dolittle.AspNetCore.Debugging.Handlers;
using Dolittle.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dolittle.AspNetCore.Generators.Documents
{
    /// <summary>
    /// An implementation of <see cref="IDebuggingHandlerDocumentGenerator"/>.
    /// </summary>
    public class DebuggingHandlerDocumentGenerator : IDebuggingHandlerDocumentGenerator
    {
        readonly ISchemaGenerator _schemaGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggingHandlerDocumentGenerator"/> class.
        /// </summary>
        /// <param name="schemaGenerator">The <see cref="ISchemaGenerator"/> used to generate schemas.</param>
        public DebuggingHandlerDocumentGenerator(ISchemaGenerator schemaGenerator)
        {
            _schemaGenerator = schemaGenerator;
        }

        /// <inheritdoc/>
        public OpenApiDocument GenerateFor(IDebuggingHandler handler)
        {
            var repository = new SchemaRepository();
            var paths = new OpenApiPaths();
            GeneratePaths(paths, handler, repository);

            return new OpenApiDocument
            {
                Info = new OpenApiInfo
                {
                    Title = handler.Title,
                },
                Paths = paths,
                Components = new OpenApiComponents
                {
                    Schemas = repository.Schemas,
                },
            };
        }

        void GeneratePaths(OpenApiPaths paths, IDebuggingHandler handler, SchemaRepository repository)
        {
            foreach ((var path, var artifact) in handler.Aritfacts)
            {
                var item = new OpenApiPathItem();

                AddGetOperation(handler, artifact, item, repository);
                AddPostOperation(handler, artifact, item, repository);

                var tag = new OpenApiTag { Name = path.ToString().Split('/')[1], };
                foreach ((_, var operation) in item.Operations)
                {
                    operation.Tags = new[] { tag };
                }

                paths.Add(path, item);
            }
        }

        void AddGetOperation(IDebuggingHandler handler, Type artifact, OpenApiPathItem item, SchemaRepository repository)
        {
            if (handler.GetType().ImplementsOpenGeneric(typeof(ICanHandleGetRequests<>)))
            {
                item.AddOperation(OperationType.Get, GenerateOperation(handler, artifact, repository));
            }
        }

        void AddPostOperation(IDebuggingHandler handler, Type artifact, OpenApiPathItem item, SchemaRepository repository)
        {
            if (handler.GetType().ImplementsOpenGeneric(typeof(ICanHandlePostRequests<>)))
            {
                item.AddOperation(OperationType.Post, GenerateOperation(handler, artifact, repository));
            }
        }

        OpenApiOperation GenerateOperation(IDebuggingHandler handler, Type artifact, SchemaRepository repository)
        {
            return new OpenApiOperation
            {
                RequestBody = new OpenApiRequestBody
                {
                    Required = true,
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        {
                            "application/json",
                            new OpenApiMediaType
                            {
                                Schema = _schemaGenerator.GenerateSchema(artifact, repository),
                            }
                        }
                    }
                },
                Responses = GenerateResponses(handler, repository),
            };
        }

        OpenApiResponses GenerateResponses(IDebuggingHandler handler, SchemaRepository repository)
        {
            var responses = new OpenApiResponses();
            foreach ((var statusCode, var description) in handler.Responses)
            {
                responses.Add($"{statusCode}", new OpenApiResponse
                {
                    Description = description,
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        {
                            "text/plain",
                            new OpenApiMediaType
                            {
                                Schema = _schemaGenerator.GenerateSchema(typeof(string), repository),
                            }
                        }
                    }
                });
            }

            return responses;
        }
    }
}