/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using Dolittle.Applications;
using Dolittle.Execution;
using Dolittle.Lifecycle;
using Dolittle.Security;
using Dolittle.Tenancy;

namespace Dolittle.AspNetCore.Execution
{
    /// <summary>
    /// Represents an implementation of <see cref="IExecutionContextConfigurator"/>
    /// </summary>
    [Singleton]
    public class ExecutionContextConfigurator : IExecutionContextConfigurator
    {
        const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
        
        readonly IExecutionContextManager _executionContextManager;

        /// <summary>
        /// Instantiates an instance of <see cref="ExecutionContextConfigurator"/>
        /// </summary>
        /// <param name="executionContextManager"></param>
        public ExecutionContextConfigurator(IExecutionContextManager executionContextManager)
        {
            _executionContextManager = executionContextManager;
        }
        /// <inheritdoc/>
        public ExecutionContext CurrentExecutionContext()
        {
            return _executionContextManager.Current;
        }
        /// <inheritdoc/>
        public ExecutionContext ConfigureFor(TenantId tenantId, CorrelationId correlationId, Claims claims)
        {
            
            var executionEnvironment = DeduceEnvironment();
            var executionApplication = DeduceApplication();
            var executionBoundedContext = DeduceBoundedContext();

            _executionContextManager.SetConstants(executionApplication, executionBoundedContext, executionEnvironment);

            return _executionContextManager.CurrentFor(tenantId, correlationId, claims ?? Dolittle.Security.Claims.Empty);
        }

        Environment DeduceEnvironment()
        {
            var aspnetcoreEnvironment = System.Environment.GetEnvironmentVariable(ASPNETCORE_ENVIRONMENT)?.ToLower() ?? "undetermined";

            switch(aspnetcoreEnvironment)
            {
                case "development":
                    return "Development";
                case "production":
                    return "Production";
                default:
                    return Environment.Undetermined;
            }
        }
        Application DeduceApplication()
        {
            return _executionContextManager.Current.Application;
        }
        BoundedContext DeduceBoundedContext()
        {
            return _executionContextManager.Current.BoundedContext;
        }
    }
}
