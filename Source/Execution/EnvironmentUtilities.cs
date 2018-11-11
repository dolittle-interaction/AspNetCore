/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using Dolittle.Execution;

namespace Dolittle.AspNetCore.Execution
{
    /// <summary>
    /// Helper methods for <see cref="System.Environment"/>
    /// </summary>
    public static class EnvironmentUtilities
    {
        const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";

        /// <summary>
        /// Get <see cref="Dolittle.Execution.Environment">execution environment</see>
        /// </summary>
        /// <returns><see cref="Dolittle.Execution.Environment">execution environment</see></returns>
        public static Dolittle.Execution.Environment GetExecutionEnvironment()
        {
            var aspnetcoreEnvironment = System.Environment.GetEnvironmentVariable(ASPNETCORE_ENVIRONMENT)?.ToLower() ?? "undetermined";
            switch(aspnetcoreEnvironment)
            {
                case "development":
                    return Environment.Development;
                case "production":
                    return Environment.Production;
            }

            return Environment.Undetermined;
        }
    }
}
