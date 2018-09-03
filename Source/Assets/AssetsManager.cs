/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dolittle.Lifecycle;
using Microsoft.AspNetCore.Hosting;

namespace Dolittle.AspNetCore.Assets
{
    /// <summary>
    /// Represents an implementation of <see cref="IAssetsManager"/>
    /// </summary>
    [Singleton]
    public class AssetsManager : IAssetsManager
    {
        Dictionary<string, List<string>> _assetsByExtension = new Dictionary<string, List<string>>();
        readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of <see cref="AssetsManager"/>
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        public AssetsManager(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Initialize();
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetFilesForExtension(string extension)
        {
            extension = MakeSureExtensionIsPrefixedWithADot(extension);
            if (!_assetsByExtension.ContainsKey(extension))return new string[0];
            var assets = _assetsByExtension[extension];
            return assets;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetStructureForExtension(string extension)
        {
            extension = MakeSureExtensionIsPrefixedWithADot(extension);
            if (!_assetsByExtension.ContainsKey(extension))return new string[0];
            var assets = _assetsByExtension[extension];
            return assets.Select(a => FormatPath(Path.GetDirectoryName(a))).Distinct().ToArray();
        }

        /// <inheritdoc/>
        public void AddAsset(string relativePath)
        {
            var extension = Path.GetExtension(relativePath);
            if (relativePath.StartsWith("/")|| relativePath.StartsWith("\\"))relativePath = relativePath.Substring(1);

            List<string> assets;
            if (!_assetsByExtension.ContainsKey(extension))
            {
                assets = new List<string>();
                _assetsByExtension[extension] = assets;
            }
            else
                assets = _assetsByExtension[extension];

            assets.Add(relativePath);
        }

        void Initialize()
        {
            _assetsByExtension = new Dictionary<string, List<string>>();
            var root = _hostingEnvironment.WebRootPath;
            var files = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var relativePath = FormatPath(file.Replace(root, string.Empty));
                AddAsset(relativePath);
            }
        }
        

        string MakeSureExtensionIsPrefixedWithADot(string extension)
        {
            if (!extension.StartsWith("."))
                return "." + extension;

            return extension;
        }

        string FormatPath(string input)
        {
            return input.Replace("\\", "/");
        }
    }
}