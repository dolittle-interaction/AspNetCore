/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Assemblies;
using Dolittle.DependencyInversion;
using Dolittle.Types;

namespace Dolittle.AspNetCore.Bootstrap
{
    /// <summary>
    /// Represents a result of booting
    /// </summary>
    public class BootResult
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BootResult"/>
        /// </summary>
        /// <param name="assemblies"><see cref="IAssemblies"/> used</param>
        /// <param name="typeFinder"><see cref="ITypeFinder"/> used</param>
        /// <param name="bindings"><see cref="IBindingCollection">Bindings</see> setup</param>
        public BootResult(
            IAssemblies assemblies,
            ITypeFinder typeFinder,
            IBindingCollection bindings)
        {
            Assemblies = assemblies;
            TypeFinder = typeFinder;
            Bindings = bindings;

        }

        /// <summary>
        /// Gets the <see cref="IAssemblies"/>
        /// </summary>
        public IAssemblies Assemblies {  get; }

        /// <summary>
        /// Gets the <see cref="ITypeFinder"/>
        /// </summary>
        public ITypeFinder TypeFinder {  get; }

        /// <summary>
        /// Gets the <see cref="IBindingCollection"/>
        /// </summary>
        public IBindingCollection Bindings {  get; }
    }
}