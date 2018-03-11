/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Dolittle.AspNetCore.Assets
{
    /// <summary>
    /// Defines the manager of assets
    /// </summary>
    public interface IAssetsManager
    {
        /// <summary>
        /// Get files for a specific extension
        /// </summary>
        /// <param name="extension">Extension to get files for</param>
        /// <returns>Files</returns>
        IEnumerable<string> GetFilesForExtension(string extension);

        /// <summary>
        /// Get structure for a specific extension
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        IEnumerable<string> GetStructureForExtension(string extension);

        /// <summary>
        /// Add a asset
        /// </summary>
        /// <param name="relativePath">Relative path to the asset</param>
        void AddAsset(string relativePath);
    }
}
