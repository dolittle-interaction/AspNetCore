/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Linq;
using System.Text;
using Dolittle.CodeGeneration;
using Dolittle.CodeGeneration.JavaScript;
using Dolittle.Execution;
using Dolittle.Strings;
using Dolittle.ReadModels;
using Dolittle.Types;

namespace Dolittle.AspNetCore.ReadModels.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadModelProxies : IReadModelProxies
    {
        readonly ITypeFinder _typeFinder;
        readonly ICodeGenerator _codeGenerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeFinder"></param>
        /// <param name="codeGenerator"></param>
        public ReadModelProxies(ITypeFinder typeFinder, ICodeGenerator codeGenerator)
        {
            _typeFinder = typeFinder;
            _codeGenerator = codeGenerator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            var typesByNamespace = _typeFinder.FindMultiple<IReadModel>().GroupBy(t => t.Namespace);

            var result = new StringBuilder();

            Namespace currentNamespace;
            Namespace globalRead = _codeGenerator.Namespace("read");

            foreach (var @namespace in typesByNamespace)
            {
                currentNamespace = globalRead;


                foreach (var type in @namespace)
                {
                    var name = type.Name.ToCamelCase();
                    currentNamespace.Content.Assign(name)
                        .WithType(t =>
                            t.WithSuper("Dolittle.read.ReadModel")
                                .Function
                                    .Body
                                        .Variant("self", v => v.WithThis())
                                        .Property("_generatedFrom", p => p.WithString(type.FullName))
                                        .WithPropertiesFrom(type, typeof(IReadModel)));

                    currentNamespace.Content.Assign("readModelOf" + name.ToPascalCase())
                        .WithType(t =>
                            t.WithSuper("Dolittle.read.ReadModelOf")
                                .Function
                                    .Body
                                        .Variant("self", v => v.WithThis())
                                        .Property("_name", p => p.WithString(name))
                                        .Property("_generatedFrom", p => p.WithString(type.FullName))
                                        .Property("_readModelType", p => p.WithLiteral(currentNamespace.Name+"." + name))
                                        .WithReadModelConvenienceFunctions(type));
                }

                if (currentNamespace != globalRead)
                    result.Append(_codeGenerator.GenerateFrom(currentNamespace));
            }
            result.Append(_codeGenerator.GenerateFrom(globalRead));
            return result.ToString();
        }
    }
}
