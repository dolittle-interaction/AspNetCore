// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dolittle.AspNetCore.Execution;
using Dolittle.Collections;
using Dolittle.Concepts;
using Dolittle.DependencyInversion;
using Dolittle.Dynamic;
using Dolittle.Execution;
using Dolittle.Logging;
using Dolittle.Queries;
using Dolittle.Queries.Coordination;
using Dolittle.Serialization.Json;
using Dolittle.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

#pragma warning disable CA1308

namespace Dolittle.AspNetCore.Queries
{
    /// <summary>
    /// Represents an API endpoint for working with <see cref="IQuery">queries</see>.
    /// </summary>
    [Route("api/Dolittle/Queries")]
    public class QueryCoordinator : ControllerBase
    {
        readonly ITypeFinder _typeFinder;
        readonly IContainer _container;
        readonly IQueryCoordinator _queryCoordinator;
        readonly IExecutionContextConfigurator _executionContextConfigurator;
        readonly IInstancesOf<IQuery> _queries;
        readonly ILogger _logger;
        readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryCoordinator"/> class.
        /// </summary>
        /// <param name="typeFinder"><see cref="ITypeFinder"/> for finding types.</param>
        /// <param name="container"><see cref="IContainer"/> for getting instances of types.</param>
        /// <param name="queryCoordinator">The underlying <see cref="IQueryCoordinator"/>.</param>
        /// <param name="executionContextConfigurator"><see cref="IExecutionContextConfigurator"/> for working with <see cref="ExecutionContext"/>.</param>
        /// <param name="queries"><see cref="IInstancesOf{T}"/> of <see cref="IQuery"/>.</param>
        /// <param name="serializer">JSON <see cref="ISerializer"/>.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public QueryCoordinator(
            ITypeFinder typeFinder,
            IContainer container,
            IQueryCoordinator queryCoordinator,
            IExecutionContextConfigurator executionContextConfigurator,
            IInstancesOf<IQuery> queries,
            ISerializer serializer,
            ILogger logger)
        {
            _typeFinder = typeFinder;
            _container = container;
            _queryCoordinator = queryCoordinator;
            _executionContextConfigurator = executionContextConfigurator;
            _queries = queries;
            _serializer = serializer;
            _logger = logger;
        }

        /// <summary>
        /// [Get] Action for performing a query.
        /// </summary>
        /// <param name="queryRequest">The <see cref="QueryRequest"/>.</param>
        /// <returns><see cref="IActionResult"/> with the query result.</returns>
        [HttpGet]
        public async Task<IActionResult> Handle([FromBody] QueryRequest queryRequest)
        {
            var content = new ContentResult();
            QueryResult queryResult = null;

            try
            {
                _logger.Information($"Executing query : {queryRequest.NameOfQuery}");
                var queryType = _typeFinder.GetQueryTypeByName(queryRequest.GeneratedFrom);
                var query = _container.Get(queryType) as IQuery;

                var properties = queryType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty).ToDictionary(p => p.Name.ToLowerInvariant(), p => p);
                CopyPropertiesFromRequestToQuery(queryRequest, query, properties);

                queryResult = await _queryCoordinator.Execute(query, new PagingInfo()).ConfigureAwait(false);
                if (queryResult.Success) AddClientTypeInformation(queryResult);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error executing query : '{queryRequest.NameOfQuery}'");
                queryResult = new QueryResult
                {
                    Exception = ex,
                    QueryName = queryRequest.NameOfQuery
                };
            }

            content.Content = _serializer.ToJson(queryResult, SerializationOptions.CamelCase);
            content.ContentType = "application/json";
            return content;
        }

        /// <summary>
        /// [GET] Action for getting available queries.
        /// </summary>
        /// <returns>All implementations of <see cref="IQuery"/>.</returns>
        [HttpGet]
        public IEnumerable<IQuery> Queries()
        {
            try
            {
                return _queries.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error listing queries.");
                throw;
            }
        }

        void AddClientTypeInformation(QueryResult result)
        {
            var items = new List<object>();
            foreach (var item in result.Items)
            {
                var dynamicItem = item.AsExpandoObject();
                items.Add(dynamicItem);
            }

            result.Items = items;
        }

        void CopyPropertiesFromRequestToQuery(QueryRequest request, object instance, Dictionary<string, PropertyInfo> properties)
        {
            request.Parameters.Keys.ForEach(propertyName =>
            {
                var lowerCasedPropertyName = propertyName.ToLowerInvariant();
                if (properties.ContainsKey(lowerCasedPropertyName))
                {
                    var property = properties[lowerCasedPropertyName];
                    object value = request.Parameters[propertyName];

                    value = HandleValue(property.PropertyType, value);
                    property.SetValue(instance, value);
                }
            });
        }

        object HandleValue(Type targetType, object value)
        {
            if (value is JArray || value is JObject)
            {
                value = _serializer.FromJson(targetType, value.ToString());
            }
            else if (targetType.IsConcept())
            {
                value = ConceptFactory.CreateConceptInstance(targetType, value);
            }
            else if (targetType == typeof(DateTimeOffset))
            {
                if (value is DateTime time)
                    value = new DateTimeOffset(time);
            }
            else if (targetType.IsEnum)
            {
                value = Enum.Parse(targetType, value.ToString());
            }
            else if (targetType == typeof(Guid))
            {
                value = Guid.Parse(value.ToString());
            }
            else
            {
                if (!targetType.IsAssignableFrom(value.GetType()))
                    value = System.Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
            }

            return value;
        }
    }
}