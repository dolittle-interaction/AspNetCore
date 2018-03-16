/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Reflection;
using Dolittle.CodeGeneration;
using Dolittle.CodeGeneration.JavaScript;
using Dolittle.Web.Configuration;
using Dolittle.Web.Proxies;

namespace Dolittle.Web.Configuration
{
    public class NamespaceConfigurationProxies : IProxyGenerator
    {
        ICodeGenerator _codeGenerator;
        WebConfiguration _configuration;

        public NamespaceConfigurationProxies(WebConfiguration configuration, ICodeGenerator codeGenerator)
        {
            _codeGenerator = codeGenerator;
            _configuration = configuration;
        }

        public string Generate()
        {
            var global = _codeGenerator
                .Global()
                    .Variant("namespaceMapper", v => v.WithFunctionCall(f=>f.WithName("Dolittle.StringMapper.create")))
                    .WithNamespaceMappersFrom(_configuration.PathsToNamespaces)
                    .AssignAccessor("Dolittle.namespaceMappers.default", a => a.WithLiteral("namespaceMapper"))
                    ;

            var result = _codeGenerator.GenerateFrom(global);
            return result;
        }
    }
}
