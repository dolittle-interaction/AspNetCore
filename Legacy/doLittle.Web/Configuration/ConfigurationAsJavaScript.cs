/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Dolittle.Configuration;
using Dolittle.Logging;
using Dolittle.Web.Assets;
using Dolittle.Web.Proxies;
using Newtonsoft.Json;

namespace Dolittle.Web.Configuration
{
    public class ConfigurationAsJavaScript
    {
        ILogger _logger;

        WebConfiguration _webConfiguration;

        string _configurationAsString;

        public ConfigurationAsJavaScript(WebConfiguration webConfiguration, ILogger logger)
        {
            _webConfiguration = webConfiguration;
            _logger = logger;
        }

        string GetResource(string name)
        {
            try
            {
                var stream = typeof(ConfigurationAsJavaScript).GetTypeInfo().Assembly.GetManifestResourceStream(name);
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                var content = Encoding.UTF8.GetString(bytes);
                return content;
            }
            catch( Exception ex )
            {
                _logger.Error(ex,$"Couldn't get the resource '{name}'");
                throw ex;
            }
        }

        public string AsString
        {
            get
            {
                if (string.IsNullOrEmpty(_configurationAsString)) Initialize();

                return _configurationAsString;
            }
        }

        

        public void Initialize()
        {
            var proxies = Configure.Instance.Container.Get<GeneratedProxies>();

            var assetsManager = Configure.Instance.Container.Get<IAssetsManager>();
            assetsManager.Initialize();

            var builder = new StringBuilder();

            if (_webConfiguration.ScriptsToInclude.JQuery)
                builder.Append(GetResource("Dolittle.Web.Scripts.jquery-2.1.3.min.js"));

            if (_webConfiguration.ScriptsToInclude.Knockout)
                builder.Append(GetResource("Dolittle.Web.Scripts.knockout-3.2.0.debug.js"));

            if (_webConfiguration.ScriptsToInclude.SignalR)
                builder.Append(GetResource("Dolittle.Web.Scripts.jquery.signalR-2.2.0.js"));

            if (_webConfiguration.ScriptsToInclude.JQueryHistory)
                builder.Append(GetResource("Dolittle.Web.Scripts.jquery.history.js"));

            if (_webConfiguration.ScriptsToInclude.Require)
            {
                builder.Append(GetResource("Dolittle.Web.Scripts.require.js"));
                builder.Append(GetResource("Dolittle.Web.Scripts.order.js"));
                builder.Append(GetResource("Dolittle.Web.Scripts.text.js"));
                builder.Append(GetResource("Dolittle.Web.Scripts.domReady.js"));
                builder.Append(GetResource("Dolittle.Web.Scripts.noext.js"));
            }

            if (_webConfiguration.ScriptsToInclude.Dolittle)
                builder.Append(GetResource("Dolittle.Web.Scripts.Dolittle.debug.js"));

            builder.Append(proxies.All);

            var files = assetsManager.GetFilesForExtension("js");
            var serialized = JsonConvert.SerializeObject(files);
            builder.AppendFormat("Dolittle.assetsManager.initializeFromAssets({0});", serialized);
            _configurationAsString = builder.ToString();
        }

    }
}
