/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.IO;
using System.Threading.Tasks;
using Dolittle.AspNetCore.Bootstrap;
using Dolittle.Booting;
using Dolittle.DependencyInversion;
using Dolittle.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders.Physical;
using System;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extensions for <see cref="IApplicationBuilder"/>
    /// </summary>
    public static class ApplicationBuilderExtensions
    {

        static ExecutionContextSetupConfigurationDelegate DefaultExecutionContextSetupConfigurationDelegate = _ => ExecutionContextSetupConfiguration.Default;
        /// <summary>
        /// Use Dolittle for the given application
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/> to use Dolittle for</param>
        /// <param name="executionContextSetupConfigurationCallback">Callback for configuring the <see cref="ExecutionContextSetup"/></param>
        public static void UseDolittle(this IApplicationBuilder app, ExecutionContextSetupConfigurationDelegate executionContextSetupConfigurationCallback = null)
        {
            app.UseDolittle(_ => ExecutionContextSetupConfiguration.Default);
            var container = app.ApplicationServices.GetService(typeof(IContainer)) as IContainer;

            var logger = app.ApplicationServices.GetService(typeof(ILogger)) as ILogger;

            Dolittle.DependencyInversion.Booting.Boot.ContainerReady(container);
            
            Dolittle.Booting.BootStages.ContainerReady(container);

            var bootProcedures = container.Get<IBootProcedures>();
            bootProcedures.Perform();
            app.UseMiddleware<HealthCheckMiddleware>();
            app.UseMiddleware<ExecutionContextSetup>(executionContextSetupConfigurationCallback);
        }

        /// <summary>
        /// Run as a single page application - typically end off your application configuration in Startup.cs with this
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/> you're building</param>
        /// <param name="pathToFile">Optional path to file that will be sent as the single page</param>
        /// <remarks>
        /// If there is no path to a file given, it will default to index.html inside your wwwwroot
        /// </remarks>
        public static void RunAsSinglePageApplication(this IApplicationBuilder app, string pathToFile = null)
        {
            var environment = app.ApplicationServices.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;

            app.Run(async context =>
            {
                if (Path.HasExtension(context.Request.Path)) await Task.CompletedTask;
                context.Request.Path = new PathString("/");

                var path = pathToFile ?? $"{environment.ContentRootPath}/wwwroot/index.html";
                await context.Response.SendFileAsync(new PhysicalFileInfo(new FileInfo(path)));
            });
        }
    }
}