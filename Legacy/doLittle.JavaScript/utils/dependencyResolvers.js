Dolittle.namespace("Dolittle", {
    dependencyResolvers: (function () {
        return {
            getAll: function () {
                var resolvers = [
                    new Dolittle.WellKnownTypesDependencyResolver(),
                    new Dolittle.DefaultDependencyResolver(),
                    new Dolittle.KnownArtifactTypesDependencyResolver(),
                    new Dolittle.KnownArtifactInstancesDependencyResolver(),

                ];
                for (var property in this) {
                    if (property.indexOf("_") !== 0 &&
                        this.hasOwnProperty(property) &&
                        typeof this[property] !== "function") {
                        resolvers.push(this[property]);
                    }
                }
                return resolvers;
            }
        };
    })()
});