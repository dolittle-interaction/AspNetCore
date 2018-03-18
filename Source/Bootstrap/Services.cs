/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using Dolittle.Assemblies;
using Dolittle.Collections;
using Dolittle.DependencyInversion;
using Dolittle.DependencyInversion.Scopes;
using Dolittle.DependencyInversion.Strategies;
using Dolittle.Logging;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Dolittle.Reflection;
using Microsoft.AspNetCore.Mvc;
using Dolittle.AspNetCore.Bootstrap;
using Dolittle.Types;

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
        public static void AddDolittle(this IServiceCollection services, ILoggerFactory loggerFactory = null)
        {
            if (loggerFactory == null) loggerFactory = new LoggerFactory();

            var logAppenders = Dolittle.Logging.Bootstrap.EntryPoint.Initialize(loggerFactory);
            var logger = new Logger(logAppenders);

            var assemblies = Dolittle.Assemblies.Bootstrap.EntryPoint.Initialize(logger);
            var typeFinder = Dolittle.Types.Bootstrap.EntryPoint.Initialize(assemblies);

            services.AddSingleton(typeof(IAssemblies), assemblies);
            services.AddSingleton(typeof(Dolittle.Logging.ILogger), logger);

            var discoveredBindings = Dolittle.DependencyInversion.Bootstrap.EntryPoint.DiscoverBindings(assemblies, typeFinder);

            var translatedServices = discoveredBindings.Select(GetServiceDescriptor);
            translatedServices.ForEach(service =>
            {
                if (!services.Any(s => s.ServiceType == service.ServiceType))
                {
                    services.Add(service);
                }
            });

            AddMvcOptions(services, typeFinder);
        }

        static void AddMvcOptions(IServiceCollection services, ITypeFinder typeFinder)
        {
            var mvcOptionsAugmenters = typeFinder.FindMultiple<ICanAddMvcOptions>();
            mvcOptionsAugmenters.ForEach(augmenterType =>
            {
                if (!augmenterType.HasDefaultConstructor()) throw new ArgumentException($"Type '{augmenterType.AssemblyQualifiedName}' is missing a default constructor");
                var augmenter = Activator.CreateInstance(augmenterType) as ICanAddMvcOptions;
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