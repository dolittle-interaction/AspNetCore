/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Dolittle.AspNetCore.Bootstrap
{

    /// <summary>
    /// The delegate that configures a <see cref="ExecutionContextSetupConfiguration"/>
    /// </summary>
    /// <param name="configuration">The <see cref="ExecutionContextSetupConfiguration"/> to modify</param>
    /// <returns></returns>
    public delegate ExecutionContextSetupConfiguration ExecutionContextSetupConfigurationDelegate(ExecutionContextSetupConfiguration configuration);
    
}
