/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Machine.Specifications;
using Microsoft.AspNetCore.Authentication;

namespace Dolittle.AspNetCore.Authentication.for_HttpHeaderHandler
{
    public class when_handling_with_automatically_discovered_command_handlers
    {
        static given.a_handler handler; 
        static AuthenticateResult result;

        Establish context = () =>
        {
            handler = new given.a_handler();
        };

        Because of = async () => result = await handler.AuthenticateAsync();

        It should_return_true_when_trying_to_handle = () => result.Succeeded.ShouldBeTrue();
    }
}