/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Dolittle.AspNetCore.Execution;
using Dolittle.Collections;
using Dolittle.Concepts;
using Dolittle.DependencyInversion;
using Dolittle.Dynamic;
using Dolittle.Logging;
using Dolittle.Queries;
using Dolittle.Queries.Coordination;
using Dolittle.Security;
using Dolittle.Serialization.Json;
using Dolittle.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Dolittle.AspNetCore.Queries
{
    /// <summary>
    /// Represents an API endpoint for working with <see cref="IQuery">queries</see>
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
        /// Initializes a new instance of <see cref="QueryCoordinator"/>
        /// </summary>
        /// <param name="typeFinder"></param>
        /// <param name="container"></param>
        /// <param name="queryCoordinator">The underlying <see cref="IQueryCoordinator"/> </param>
        /// <param name="executionContextConfigurator"></param>
        /// <param name="queries"></param>
        /// <param name="serializer"></param>
        /// <param name="logger"></param>
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
        /// 
        /// </summary>
        /// <param name="queryRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Handle([FromBody] QueryRequest queryRequest)
        {
            var content = new ContentResult();
            QueryResult queryResult = null;
            try
            {
                // _executionContextConfigurator.ConfigureFor(_tenantResolver.Resolve(HttpContext.Request), Dolittle.Execution.CorrelationId.New(), ClaimsPrincipal.Current.ToClaims());
                _logger.Information($"Executing query : {queryRequest.NameOfQuery}");
                var queryType = _typeFinder.GetQueryTypeByName(queryRequest.GeneratedFrom);
                var query = _container.Get(queryType) as IQuery;

                var properties = queryType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty).ToDictionary(p => p.Name.ToLowerInvariant(), p => p);
                CopyPropertiesFromRequestToQuery(queryRequest, query, properties);

                queryResult = await _queryCoordinator.Execute(query, new PagingInfo());
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

        void AddClientTypeInformation(QueryResult result)
        {
            var items = new List<object>();
            foreach (var item in result.Items)
            {
                var dynamicItem = item.AsExpandoObject();
                var type = item.GetType();
                items.Add(dynamicItem);
            }
            result.Items = items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<IQuery> Queries()
        {
            try
            {
                // _executionContextConfigurator.ConfigureFor(_tenantResolver.Resolve(HttpContext.Request), Dolittle.Execution.CorrelationId.New(), ClaimsPrincipal.Current.ToClaims());
                return _queries.ToList();
            }
            catch(Exception ex)
            {

                _logger.Error(ex, $"Error listing queries.");
                throw;
            }
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
            if (value is JArray ||  value is JObject)
            {
                value = _serializer.FromJson(targetType, value.ToString());
            }
            else if (targetType.IsConcept())
            {
                value = ConceptFactory.CreateConceptInstance(targetType, value);
            }
            else if (targetType == typeof(DateTimeOffset))
            {
                if(value is DateTime time)
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
                    value = System.Convert.ChangeType(value, targetType);
            }

            return value;
        }
    }
}