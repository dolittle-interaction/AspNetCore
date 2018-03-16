/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Text;
using System.Linq;
using Dolittle.CodeGeneration;
using Dolittle.CodeGeneration.JavaScript;
using Dolittle.Strings;
using System.Reflection;
using Dolittle.Types;
using Dolittle.Queries;

namespace Dolittle.AspNetCore.Queries.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryProxies : IQueryProxies
    {
        ITypeFinder _typeFinder;
        ICodeGenerator _codeGenerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeDiscoverer"></param>
        /// <param name="codeGenerator"></param>
        public QueryProxies(ITypeFinder typeDiscoverer, ICodeGenerator codeGenerator)
        {
            _typeFinder = typeDiscoverer;
            _codeGenerator = codeGenerator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            var typesByNamespace = _typeFinder.FindMultiple(typeof(IQueryFor<>)).GroupBy(t => t.Namespace);

            var result = new StringBuilder();

            Namespace currentNamespace;
            Namespace globalRead = _codeGenerator.Namespace("read");

            foreach (var @namespace in typesByNamespace)
            {
                currentNamespace = globalRead;

                foreach (var type in @namespace)
                {
                    var name = type.Name.ToCamelCase();
                    var queryForTypeName = type.GetTypeInfo().GetInterface(typeof(IQueryFor<>).Name).GetGenericArguments()[0].Name.ToCamelCase();
                    currentNamespace.Content.Assign(name)
                        .WithType(t =>
                            t.WithSuper("Dolittle.read.Query")
                                .Function
                                    .Body
                                        .Variant("self", v => v.WithThis())
                                        .Property("_name", p => p.WithString(name))
                                        .Property("_generatedFrom", p => p.WithString(type.FullName))
                                        .Property("_readModel", p => p.WithLiteral(currentNamespace.Name + "." + queryForTypeName))
                                        .WithObservablePropertiesFrom(type, excludePropertiesFrom: typeof(IQueryFor<>), propertyVisitor: (p) => p.Name != "Query"));

                }
                if (currentNamespace != globalRead)
                    result.Append(_codeGenerator.GenerateFrom(currentNamespace));
            }

            result.Append(_codeGenerator.GenerateFrom(globalRead));
            return result.ToString();
        }

    }
}
