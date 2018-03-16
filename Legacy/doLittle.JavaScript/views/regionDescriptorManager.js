Dolittle.namespace("Dolittle.views", {
    regionDescriptorManager: Dolittle.Singleton(function () {
        /// <summary>Represents a manager that knows how to manage region descriptors</summary>

        this.describe = function (view, region) {
            /// <summary>Describe a specific region related to a view</summary>
            /// <param name="view" type="Dolittle.views.View">View related to the region</param>
            /// <param name="region" type="Dolittle.views.Region">Region that needs to be described</param>
            var promise = Dolittle.execution.Promise.create();
            var localPath = Dolittle.Path.getPathWithoutFilename(view.path);
            var namespacePath = Dolittle.namespaceMappers.mapPathToNamespace(localPath);
            if (namespacePath != null) {
                var namespace = Dolittle.namespace(namespacePath);

                Dolittle.views.Region.current = region;
                Dolittle.dependencyResolver.beginResolve(namespace, "RegionDescriptor").continueWith(function (descriptor) {
                    descriptor.describe(region);
                    promise.signal();
                }).onFail(function () {
                    promise.signal();
                });
            } else {
                promise.signal();
            }
            return promise;
        };

        this.describeTopLevel = function (region) {

        };
    })
});