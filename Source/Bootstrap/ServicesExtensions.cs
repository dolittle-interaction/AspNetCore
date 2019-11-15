/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using Dolittle.AspNetCore.Authentication;
using Dolittle.AspNetCore.Bootstrap;
using DolittleOptions = Dolittle.AspNetCore.Configuration.Options;
using Dolittle.AspNetCore.Execution;
using Dolittle.Booting;
using Dolittle.Collections;
using Dolittle.DependencyInversion;
using Dolittle.Reflection;
using Dolittle.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dolittle.Serialization.Json;

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
            return AddDolittle(services, null, loggerFactory);
        }

        /// <summary>
        /// Adds Dolittle services
        /// </summary>
        /// <returns></returns>
        public static BootloaderResult AddDolittle(this IServiceCollection services, Action<DolittleOptions> configure, ILoggerFactory loggerFactory = null)
        {
            return AddDolittle(services, null, loggerFactory, configure);
        }

        /// <summary>
        /// Adds Dolittle services
        /// </summary>
        /// <returns></returns>
        public static BootloaderResult AddDolittle(this IServiceCollection services, Action<IBootBuilder> builderDelegate, ILoggerFactory loggerFactory = null, Action<DolittleOptions> configure = null)
        {
            var bootloader = Bootloader.Configure(_ => {
                if( loggerFactory != null ) _ = _.UseLoggerFactory(loggerFactory);
                if( EnvironmentUtilities.GetExecutionEnvironment() == Dolittle.Execution.Environment.Development ) _ = _.Development();
                _.SkipBootprocedures()
                .UseContainer<Container>();
                if ( builderDelegate != null) builderDelegate(_);
            });

            var bootloaderResult = bootloader.Start();

            if (configure != null) services.Configure<DolittleOptions>(configure);

            AddAuthentication(services);
            AddMvcOptions(services, bootloaderResult.TypeFinder, bootloaderResult.Container);

            return bootloaderResult;
        }  

        static void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication("Dolittle.Headers")
                .AddScheme<HttpHeaderSchemeOptions, HttpHeaderHandler>("Dolittle.Headers", _ => {});
        }
        
        static void SetupUnhandledExceptionHandler(Dolittle.Logging.ILogger logger)
        {
            AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs args) => 
            {
                var exception = (Exception) args.ExceptionObject;
                logger.Error(exception, "Unhandled Exception");
            };
        }
        static void AddMvcOptions(IServiceCollection services, ITypeFinder typeFinder, IContainer container)
        {
            var mvcOptionsAugmenters = typeFinder.FindMultiple<ICanAddMvcOptions>();
            mvcOptionsAugmenters.ForEach(augmenterType =>
            {
                if (!augmenterType.HasDefaultConstructor()) throw new ArgumentException($"Type '{augmenterType.AssemblyQualifiedName}' is missing a default constructor");
                var augmenter = Activator.CreateInstance(augmenterType) as ICanAddMvcOptions;
                services.Configure<MvcOptions>(augmenter.Add);
            });

            var providers = container.Get<IInstancesOf<ICanProvideConverters>>();
            var converters = providers.SelectMany(_ => _.Provide());
            services.Configure<MvcJsonOptions>(_ => {
                converters.ForEach(_.SerializerSettings.Converters.Add);
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