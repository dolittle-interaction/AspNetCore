// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Hosting.Microsoft;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Debugging
{
    /// <summary>
    /// The entrypoint.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Arguments for the process.</param>
        public static void Main(string[] args) =>
            CreateHostBuilder(args)
                .Build()
                .Run();

        /// <summary>
        /// Create a host builder.
        /// </summary>
        /// <param name="args">Arguments for the process.</param>
        /// <returns>Host builder to build and run.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseDolittle()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseEnvironment("Development")
                        .UseStartup<Startup>();
                });
    }
}
