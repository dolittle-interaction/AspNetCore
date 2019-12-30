// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.AspNetCore.Authentication;
using Dolittle.AspNetCore.Bootstrap;
using Dolittle.AspNetCore.Execution;
using Dolittle.Booting;
using Dolittle.Collections;
using Dolittle.Reflection;
using Dolittle.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DolittleOptions = Dolittle.AspNetCore.Configuration.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for services.
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Adds Dolittle services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add Dolittle to.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use, default is null - which means it will use the default factory.</param>
        /// <returns>The <see cref="BootloaderResult"/>.</returns>
        public static BootloaderResult AddDolittle(this IServiceCollection services, ILoggerFactory loggerFactory = null)
        {
            return AddDolittle(services, null, loggerFactory, null, null);
        }

        /// <summary>
        /// Adds Dolittle services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add Dolittle to.</param>
        /// <param name="configure">Callback for configuring <see cref="DolittleOptions"/>.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use, default is null - which means it will use the default factory.</param>
        /// <returns>The <see cref="BootloaderResult"/>.</returns>
        public static BootloaderResult AddDolittle(
            this IServiceCollection services,
            Action<DolittleOptions> configure,
            ILoggerFactory loggerFactory = null)
        {
            return AddDolittle(services, null, loggerFactory, configure, null);
        }

        /// <summary>
        /// Adds Dolittle services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add Dolittle to.</param>
        /// <param name="configureAuthentication">Callback for configuring <see cref="HttpHeaderSchemeOptions"/>.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use, default is null - which means it will use the default factory.</param>
        /// <returns>The <see cref="BootloaderResult"/>.</returns>
        public static BootloaderResult AddDolittle(
            this IServiceCollection services,
            Action<HttpHeaderSchemeOptions> configureAuthentication,
            ILoggerFactory loggerFactory = null)
        {
            return AddDolittle(services, null, loggerFactory, null, configureAuthentication);
        }

        /// <summary>
        /// Adds Dolittle services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add Dolittle to.</param>
        /// <param name="builderDelegate">Callback for building on the <see cref="IBootBuilder"/>.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use, default is null - which means it will use the default factory.</param>
        /// <param name="configure">Callback for configuring <see cref="DolittleOptions"/>.</param>
        /// <param name="configureAuthentication">Callback for configuring <see cref="HttpHeaderSchemeOptions"/>.</param>
        /// <returns>The <see cref="BootloaderResult"/>.</returns>
        public static BootloaderResult AddDolittle(
            this IServiceCollection services,
            Action<IBootBuilder> builderDelegate,
            ILoggerFactory loggerFactory = null,
            Action<DolittleOptions> configure = null,
            Action<HttpHeaderSchemeOptions> configureAuthentication = null)
        {
            var bootloader = Bootloader.Configure(_ =>
            {
                if (loggerFactory != null) _ = _.UseLoggerFactory(loggerFactory);
                if (EnvironmentUtilities.GetExecutionEnvironment() == Dolittle.Execution.Environment.Development) _ = _.Development();
                _.SkipBootprocedures()
                .UseContainer<Container>();
                builderDelegate?.Invoke(_);
            });

            var bootloaderResult = bootloader.Start();

            if (configure != null) services.Configure(configure);

            AddAuthentication(services, configureAuthentication);
            services.AddMvc();
            AddMvcOptions(services, bootloaderResult.TypeFinder);

            return bootloaderResult;
        }

        static void AddAuthentication(IServiceCollection services, Action<HttpHeaderSchemeOptions> configure = null)
        {
            if (configure == null)
            {
                configure = _ => { };
            }

            services.AddAuthentication("Dolittle.Headers").AddScheme<HttpHeaderSchemeOptions, HttpHeaderHandler>("Dolittle.Headers", configure);
        }

        static void AddMvcOptions(IServiceCollection services, ITypeFinder typeFinder)
        {
            var mvcOptionsAugmenters = typeFinder.FindMultiple<ICanAddMvcOptions>();
            mvcOptionsAugmenters.ForEach(augmenterType =>
            {
                if (!augmenterType.HasDefaultConstructor()) throw new MissingDefaultConstructorForAugmenter(augmenterType);
                var augmenter = Activator.CreateInstance(augmenterType) as ICanAddMvcOptions;
                services.Configure<MvcOptions>(augmenter.Add);
            });
        }
    }
}