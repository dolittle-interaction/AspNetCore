// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Threading.Tasks;
using Dolittle.AspNetCore.Bootstrap;
using Dolittle.AspNetCore.Execution;
using Dolittle.Booting;
using Dolittle.DependencyInversion;
using Dolittle.DependencyInversion.Booting;
using Dolittle.Logging.Internal;
using Dolittle.Logging.Microsoft;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extensions for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Use Dolittle for the given application.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/> to use Dolittle for.</param>
        public static void UseDolittle(this IApplicationBuilder app)
        {
            var container = app.ApplicationServices.GetService(typeof(IContainer)) as IContainer;
            BootContainer.ContainerReady(container);
            BootStages.ContainerReady(container);

            var loggerFactory = container.Get<ILoggerFactory>();
            if (loggerFactory == null)
            {
#pragma warning disable CA2000
                loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
#pragma warning restore CA2000
            }

            LoggerManager.Instance.AddLogMessageWriterCreators(new LogMessageWriterCreator(loggerFactory));

            var bootProcedures = container.Get<IBootProcedures>();
            bootProcedures.Perform();
            app.UseAuthentication();
            app.UseMiddleware<ExecutionContextSetup>();
            app.UseMiddleware<HealthCheckMiddleware>();
        }

        /// <summary>
        /// Run as a single page application - typically end off your application configuration in Startup.cs with this.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/> you're building.</param>
        /// <param name="pathToFile">Optional path to file that will be sent as the single page.</param>
        /// <remarks>
        /// If there is no path to a file given, it will default to index.html inside your wwwwroot.
        /// </remarks>
        public static void RunAsSinglePageApplication(this IApplicationBuilder app, string pathToFile = null)
        {
            var environment = app.ApplicationServices.GetService(typeof(IWebHostEnvironment)) as IWebHostEnvironment;

            app.Run(async context =>
            {
                if (Path.HasExtension(context.Request.Path)) await Task.CompletedTask.ConfigureAwait(false);
                context.Request.Path = new PathString("/");

                var path = pathToFile ?? $"{environment.ContentRootPath}/wwwroot/index.html";
                await context.Response.SendFileAsync(new PhysicalFileInfo(new FileInfo(path))).ConfigureAwait(false);
            });
        }
    }
}