/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using doLittle.Assemblies;
using doLittle.Collections;
using doLittle.DependencyInversion;
using doLittle.DependencyInversion.Scopes;
using doLittle.DependencyInversion.Strategies;
using doLittle.Logging;
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
        public static void AddDolittle(this IServiceCollection services, ILoggerFactory loggerFactory = null)
        {
            if (loggerFactory == null)loggerFactory = new LoggerFactory();

            var logAppenders = doLittle.Logging.Bootstrap.EntryPoint.Initialize(loggerFactory);
            var logger = new Logger(logAppenders);

            var assemblies = doLittle.Assemblies.Bootstrap.EntryPoint.Initialize(logger);
            var typeFinder = doLittle.Types.Bootstrap.EntryPoint.Initialize(assemblies);

            services.AddSingleton(typeof(IAssemblies), assemblies);
            services.AddSingleton(typeof(doLittle.Logging.ILogger), logger);

            var discoveredBindings = doLittle.DependencyInversion.Bootstrap.EntryPoint.DiscoverBindings(assemblies, typeFinder);

            var translatedServices = discoveredBindings.Select(GetServiceDescriptor);
            translatedServices.ForEach(service =>
            {
                if (!services.Any(s => s.ServiceType == service.ServiceType))
                {
                    services.Add(service);
                }
            });

        }

        static ServiceDescriptor GetServiceDescriptor(Binding binding)
        {
            if (binding.Strategy is doLittle.DependencyInversion.Strategies.Constant)
                return new ServiceDescriptor(binding.Service, ((doLittle.DependencyInversion.Strategies.Constant)binding.Strategy).Target);

            if (binding.Strategy is doLittle.DependencyInversion.Strategies.Type)
                return new ServiceDescriptor(binding.Service, ((doLittle.DependencyInversion.Strategies.Type)binding.Strategy).Target, GetServiceLifetimeFor(binding));

            if (binding.Strategy is doLittle.DependencyInversion.Strategies.Callback)
                return new ServiceDescriptor(binding.Service, (IServiceProvider provider)=>((doLittle.DependencyInversion.Strategies.Callback)binding.Strategy).Target(), GetServiceLifetimeFor(binding));

            throw new ArgumentException("Couldn't translate to a valid service descriptor");
        }

        static ServiceLifetime GetServiceLifetimeFor(Binding binding)
        {
            if (binding.Scope is doLittle.DependencyInversion.Scopes.Singleton)return ServiceLifetime.Singleton;
            return ServiceLifetime.Transient;
        }
    }
}