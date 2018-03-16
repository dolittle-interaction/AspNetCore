/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Linq;
using System.Text;
using Dolittle.DependencyInversion;
using Dolittle.Execution;
using Dolittle.Types;
using Dolittle.Web.Commands;
using Dolittle.Web.Configuration;
#if(NET461)
using Dolittle.Web.Hubs;
#endif
using Dolittle.Web.Read;
using Dolittle.Web.Services;

namespace Dolittle.Web.Proxies
{
    [Singleton]
    public class GeneratedProxies
    {
        public GeneratedProxies(
            CommandProxies commandProxies,
            CommandSecurityProxies commandSecurityProxies,
            QueryProxies queryProxies,
            ReadModelProxies readModelProxies,
            ServiceProxies serviceProxies,
            NamespaceConfigurationProxies namespaceConfigurationProxies,
#if(NET461)
            HubProxies hubProxies,
#endif
            ITypeFinder typeFinder,
            IContainer container)
        {
            var builder = new StringBuilder();
            builder.Append(commandProxies.Generate());
            builder.Append(commandSecurityProxies.Generate());
            builder.Append(readModelProxies.Generate());
            builder.Append(queryProxies.Generate());
            builder.Append(serviceProxies.Generate());
            builder.Append(namespaceConfigurationProxies.Generate());
#if(NET461)
            builder.Append(hubProxies.Generate());
#endif

            var generatorTypes = typeFinder.FindMultiple<IProxyGenerator>().Where(t => !t.Namespace.StartsWith("Dolittle"));
            foreach (var generatorType in generatorTypes)
            {
                var generator = container.Get(generatorType) as IProxyGenerator;
                builder.Append(generator.Generate());
            }

            All = builder.ToString();
        }

        public string All { get; private set; }
    }
}
