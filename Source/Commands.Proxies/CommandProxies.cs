/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Dolittle.Artifacts;
using Dolittle.CodeGeneration;
using Dolittle.CodeGeneration.JavaScript;
using Dolittle.Commands;
using Dolittle.Strings;
using Dolittle.Types;

namespace Dolittle.AspNetCore.Commands.Proxies
{
    /// <summary>
    /// Represents a system for generating <see cref="ICommand"/> proxies
    /// </summary>
    public class CommandProxies : ICommandProxies
    {
        internal static List<string> _namespacesToExclude = new List<string>();

        IArtifactTypeMap _artifactTypeMap;
        ITypeFinder _typeFinder;
        IInstancesOf<ICanExtendCommandProperty> _commandPropertyExtenders;
        ICodeGenerator _codeGenerator;

        static CommandProxies()
        {
            ExcludeCommandsStartingWithNamespace("Dolittle");
        }

        /// <summary>
        /// Exclude specific namespace from generation
        /// </summary>
        /// <param name="namespace">Namespace to exclude</param>
        public static void ExcludeCommandsStartingWithNamespace(string @namespace)
        {
            _namespacesToExclude.Add(@namespace);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artifactTypeMap"></param>
        /// <param name="typeFinder"></param>
        /// <param name="commandPropertyExtenders"></param>
        /// <param name="codeGenerator"></param>
        public CommandProxies(
            IArtifactTypeMap artifactTypeMap,
            ITypeFinder typeFinder, 
            IInstancesOf<ICanExtendCommandProperty> commandPropertyExtenders,
            ICodeGenerator codeGenerator)
        {
            _artifactTypeMap = artifactTypeMap;
            _typeFinder = typeFinder;
            _commandPropertyExtenders = commandPropertyExtenders;
            _codeGenerator = codeGenerator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            var typesByNamespace = _typeFinder.FindMultiple<ICommand>().Where(t => !_namespacesToExclude.Any(n => t.Namespace.StartsWith(n))).GroupBy(t=>t.Namespace);

            var result = new StringBuilder();

            Namespace currentNamespace;
            Namespace globalCommands = _codeGenerator.Namespace("commands");

            foreach (var @namespace in typesByNamespace)
            {
                currentNamespace = globalCommands;
                
                foreach (var type in @namespace)
                {
                    if (type.GetTypeInfo().IsGenericType) continue;

                    var identifier = _artifactTypeMap.GetArtifactFor(type);

                    var name = type.Name.ToCamelCase();
                    currentNamespace.Content.Assign(name)
                        .WithType(t =>
                            t.WithSuper("Dolittle.commands.Command")
                                .Function
                                    .Body
                                        .Variant("self", v => v.WithThis())
                                        .Property("_commandType", p => p.WithString(identifier.Id.ToString()))

                                        .WithObservablePropertiesFrom(type, excludePropertiesFrom: typeof(ICommand), observableVisitor: (propertyName, observable) =>
                                        {
                                            foreach (var commandPropertyExtender in _commandPropertyExtenders)
                                                commandPropertyExtender.Extend(type, propertyName, observable);
                                        }));
                }

                if (currentNamespace != globalCommands)
                    result.Append(_codeGenerator.GenerateFrom(currentNamespace));
            }
            result.Append(_codeGenerator.GenerateFrom(globalCommands));
            
            return result.ToString();
        }
    }
}
