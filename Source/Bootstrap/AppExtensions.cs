/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.IO;
using System.Threading.Tasks;
using Dolittle.Runtime.Events.Coordination;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders.Physical;

namespace Microsoft.AspNetCore.Builder
 {
     /// <summary>
     /// Extensions for <see cref="IApplicationBuilder"/>
     /// </summary>
     public static class ApplicationBuilderExtensions
     {

         /// <summary>
         /// Use Dolittle for the given application
         /// </summary>
         /// <param name="app"><see cref="IApplicationBuilder"/> to use Dolittle for</param>
         public static void UseDolittle(this IApplicationBuilder app)
         {
            var committedEventStreamCoordinator = app.ApplicationServices.GetService(typeof(ICommittedEventStreamCoordinator))as ICommittedEventStreamCoordinator;
            committedEventStreamCoordinator.Initialize();
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
                if( Path.HasExtension(context.Request.Path)) await Task.CompletedTask;
                context.Request.Path = new PathString("/");
                
                var path = pathToFile ?? $"{environment.ContentRootPath}/wwwroot/index.html";
                await context.Response.SendFileAsync(new PhysicalFileInfo(new FileInfo(path)));
            });            
         }
     }
 }