/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;

namespace Dolittle.AspNetCore.Authentication.for_HttpHeaderHandler.given
{
    public class a_handler : HttpHeaderHandler
    {

        protected static Mock<IOptionsMonitor<HttpHeaderSchemeOptions>> options_monitor;
        protected static Mock<ILoggerFactory> logger_factory;
        protected static Mock<UrlEncoder> url_encoder;
        protected static Mock<ISystemClock> system_clock;
        protected static Mock<Dolittle.Logging.ILogger> logger;
        
        public a_handler(): base(Mock.Of<IOptionsMonitor<HttpHeaderSchemeOptions>>(), Mock.Of<ILoggerFactory>(), Mock.Of<UrlEncoder>(), Mock.Of<ISystemClock>(), Mock.Of<Dolittle.Logging.ILogger>())
        {


        }


        public void add_header(string key, StringValues values) 
        {
            Request.Headers.Add(key, values);
        } 
        public void clear_headers()
        {
            Request.Headers.Clear();
        }
    }
}