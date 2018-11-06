/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Dolittle.AspNetCore.Bootstrap;
using Dolittle.Assemblies;
using Dolittle.Collections;
using Dolittle.DependencyInversion;
using Dolittle.DependencyInversion.Bootstrap;
using Dolittle.DependencyInversion.Scopes;
using Dolittle.DependencyInversion.Strategies;
using Dolittle.Execution;
using Dolittle.Logging;
using Dolittle.Reflection;
using Dolittle.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BootResult = Dolittle.AspNetCore.Bootstrap.BootResult;
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for services
    /// </summary>
    public static class ServicesExtensions
    {
        static readonly AsyncLocal<LoggingContext>  _currentLoggingContext = new AsyncLocal<LoggingContext>();
        static IExecutionContextManager _executionContextManager;
        static Dolittle.Execution.ExecutionContext _initialExecutionContext;

        /// <summary>
        /// Adds Dolittle services
        /// </summary>
        /// <returns></returns>
        public static BootResult AddDolittle(this IServiceCollection services, ILoggerFactory loggerFactory = null)
        {
            _initialExecutionContext = ExecutionContextManager.SetInitialExecutionContext();

            if (loggerFactory == null)loggerFactory = new LoggerFactory();

            var logAppenders = Dolittle.Logging.Bootstrap.EntryPoint.Initialize(loggerFactory, GetCurrentLoggingContext, GetRunningEnvironment());
            var logger = new Logger(logAppenders);

            SetupUnhandledExceptionHandler(logger);
            services.AddSingleton(typeof(Dolittle.Logging.ILogger), logger);
            


            var assemblies = Dolittle.Assemblies.Bootstrap.EntryPoint.Initialize(logger);
            var typeFinder = Dolittle.Types.Bootstrap.EntryPoint.Initialize(assemblies);
            Dolittle.Resources.Configuration.Bootstrap.EntryPoint.Initialize(typeFinder);
            
            var bindings = Dolittle.DependencyInversion.Bootstrap.Boot.Start(assemblies, typeFinder, logger, typeof(Container));
            
            AddMvcOptions(services, typeFinder);
            

            return new BootResult(assemblies, typeFinder, bindings);
        }
        static LoggingContext GetCurrentLoggingContext()
        {
            Dolittle.Execution.ExecutionContext executionContext = null;

            if( _executionContextManager == null && Container != null )
                _executionContextManager = Container.Get<IExecutionContextManager>();

            if (LoggingContextIsSet())
            {
                if (_executionContextManager != null) SetLatestLoggingContext();
                return _currentLoggingContext.Value;
            }
            
            if( _executionContextManager != null ) executionContext = _executionContextManager.Current;
            else executionContext = _initialExecutionContext;

            var loggingContext = CreateLoggingContextFrom(executionContext);
            _currentLoggingContext.Value = loggingContext;

            return loggingContext;
        }
        static bool LoggingContextIsSet() => 
            _currentLoggingContext != null && _currentLoggingContext.Value != null;

        static void SetLatestLoggingContext() => 
            _currentLoggingContext.Value = CreateLoggingContextFrom(_executionContextManager.Current);
            
        
        static LoggingContext CreateLoggingContextFrom(Dolittle.Execution.ExecutionContext executionContext) =>
            new LoggingContext {
                Application = executionContext.Application,
                BoundedContext = executionContext.BoundedContext,
                CorrelationId = executionContext.CorrelationId,
                Environment = executionContext.Environment,
                TenantId = executionContext.Tenant
            };
        internal static IContainer Container;
        
        static void SetupUnhandledExceptionHandler(Dolittle.Logging.ILogger logger)
        {
            AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs args) => 
            {
                var exception = (Exception) args.ExceptionObject;
                logger.Error(exception, "Unhandled Exception");
            };
        }
        static void AddMvcOptions(IServiceCollection services, ITypeFinder typeFinder)
        {
            var mvcOptionsAugmenters = typeFinder.FindMultiple<ICanAddMvcOptions>();
            mvcOptionsAugmenters.ForEach(augmenterType =>
            {
                if (!augmenterType.HasDefaultConstructor())throw new ArgumentException($"Type '{augmenterType.AssemblyQualifiedName}' is missing a default constructor");
                var augmenter = Activator.CreateInstance(augmenterType)as ICanAddMvcOptions;
                services.Configure<MvcOptions>(augmenter.Add);
            });
        }

        static ServiceDescriptor GetServiceDescriptor(Binding binding)
        {
            if (binding.Strategy is Dolittle.DependencyInversion.Strategies.Constant)
                return new ServiceDescriptor(binding.Service, ((Dolittle.DependencyInversion.Strategies.Constant)binding.Strategy).Target);

            if (binding.Strategy is Dolittle.DependencyInversion.Strategies.Type)
                return new ServiceDescriptor(binding.Service, ((Dolittle.DependencyInversion.Strategies.Type)binding.Strategy).Target, GetServiceLifetimeFor(binding));

            if (binding.Strategy is Dolittle.DependencyInversion.Strategies.Callback)
                return new ServiceDescriptor(binding.Service, (IServiceProvider provider)=>((Dolittle.DependencyInversion.Strategies.Callback)binding.Strategy).Target(), GetServiceLifetimeFor(binding));

            throw new ArgumentException("Couldn't translate to a valid service descriptor");
        }

        static ServiceLifetime GetServiceLifetimeFor(Binding binding)
        {
            if (binding.Scope is Dolittle.DependencyInversion.Scopes.Singleton)return ServiceLifetime.Singleton;
            return ServiceLifetime.Transient;
        }
        static Dolittle.Execution.Environment GetRunningEnvironment()
        {
            var aspnetcoreEnvironment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() ?? "undetermined";

            switch(aspnetcoreEnvironment)
            {
                case "development":
                    return "Development";
                case "production":
                    return "Production";
                default:
                    return Dolittle.Execution.Environment.Undetermined;
            }
        }
    }
}