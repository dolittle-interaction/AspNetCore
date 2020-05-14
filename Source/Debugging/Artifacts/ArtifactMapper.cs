// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Dolittle.ApplicationModel;
using Dolittle.Applications.Configuration;
using Dolittle.Artifacts;
using Dolittle.Artifacts.Configuration;
using Microsoft.AspNetCore.Http;

namespace Dolittle.AspNetCore.Debugging.Artifacts
{
    /// <summary>
    /// Implementation of an <see cref="IArtifactMapper{T}"/>.
    /// </summary>
    /// <typeparam name="TArtifact">Type of artifact.</typeparam>
    public class ArtifactMapper<TArtifact> : IArtifactMapper<TArtifact>
        where TArtifact : class
    {
        readonly Topology _topology;
        readonly IArtifactTypeMap _artifactTypeMap;
        readonly ArtifactsConfiguration _artifacts;
        readonly IDictionary<Type, PathString> _artifactPaths;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtifactMapper{T}"/> class.
        /// </summary>
        /// <param name="topology"><see cref="Topology"/> configuration.</param>
        /// <param name="artifacts">The <see cref="ArtifactsConfiguration"/>.</param>
        /// <param name="artifactTypeMap"><see cref="IArtifactTypeMap"/> for mapping artifacts and types.</param>
        public ArtifactMapper(
            Topology topology,
            ArtifactsConfiguration artifacts,
            IArtifactTypeMap artifactTypeMap)
        {
            _topology = topology;
            _artifacts = artifacts;
            _artifactTypeMap = artifactTypeMap;

            _artifactPaths = new Dictionary<Type, PathString>();
            BuildMapOfArtifacts();
        }

        /// <inheritdoc/>
        public IEnumerable<Type> Artifacts => _artifactPaths.Keys;

        /// <inheritdoc/>
        public PathString GetPathFor(Type artifact)
        {
            if (_artifactPaths.TryGetValue(artifact, out var path))
            {
                return path;
            }

            throw new UnknownArtifact(artifact, typeof(TArtifact));
        }

        void BuildMapOfArtifacts()
        {
            if (_topology.Modules.Count > 0)
            {
                foreach (var module in _topology.Modules.OrderBy(_ => _.Value.Name))
                {
                    AddFeaturesRecursively(module.Value.Features, $"/{module.Value.Name}");
                }
            }
            else
            {
                AddFeaturesRecursively(_topology.Features, PathString.Empty);
            }
        }

        void AddFeaturesRecursively(IReadOnlyDictionary<Feature, FeatureDefinition> features, PathString prefix)
        {
            foreach (var feature in features)
            {
                if (_artifacts.TryGetValue(feature.Key, out var artifacts))
                {
                    AddArtifacts(
                        prefix.Add($"/{feature.Value.Name}"),
                        artifacts.Commands,
                        artifacts.EventSources,
                        artifacts.Events,
                        artifacts.Queries,
                        artifacts.ReadModels);
                }

                AddFeaturesRecursively(feature.Value.SubFeatures, prefix.Add($"/{feature.Value.Name}"));
            }
        }

        void AddArtifacts(PathString prefix, params IReadOnlyDictionary<ArtifactId, ArtifactDefinition>[] artifactsByTypes)
        {
            foreach (var artifactByType in artifactsByTypes)
            {
                foreach (var artifactDefinition in artifactByType)
                {
                    var artifactType = artifactDefinition.Value.Type.GetActualType();
                    if (typeof(TArtifact).IsAssignableFrom(artifactType))
                    {
                        _artifactPaths.Add(artifactType, prefix.Add($"/{artifactType.Name}"));
                    }
                }
            }
        }
    }
}