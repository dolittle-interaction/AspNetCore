/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Dolittle.AspNetCore.Bootstrap;
using Dolittle.AspNetCore.Execution;
using Dolittle.Assemblies;
using Dolittle.Bootstrapping;
using Dolittle.Collections;
using Dolittle.DependencyInversion;
using Dolittle.DependencyInversion.Bootstrap;
using Dolittle.DependencyInversion.Scopes;
using Dolittle.DependencyInversion.Strategies;
using Dolittle.Execution;
using Dolittle.Logging;
using Dolittle.Reflection;
using Dolittle.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for services
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Adds Dolittle services
        /// </summary>
        /// <returns></returns>
        public static BootloaderResult AddDolittle(this IServiceCollection services, ILoggerFactory loggerFactory = null)
        {
            var bootloader = Bootloader.Configure()
                .UseContainer<Container>();

            if( loggerFactory != null ) bootloader = bootloader.UseLoggerFactory(loggerFactory);

            if( EnvironmentUtilities.GetExecutionEnvironment() == Dolittle.Execution.Environment.Development ) bootloader = bootloader.Development();

            var bootloaderResult = bootloader.Start();

            AddMvcOptions(services, bootloaderResult.TypeFinder);

            return bootloaderResult;
        }
        
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
    }
}