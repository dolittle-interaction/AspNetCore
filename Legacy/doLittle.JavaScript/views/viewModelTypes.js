Dolittle.namespace("Dolittle.views", {
    viewModelTypes: Dolittle.Singleton(function () {
        var self = this;

        function getNamespaceFrom(path) {
            var localPath = Dolittle.Path.getPathWithoutFilename(path);
            var namespacePath = Dolittle.namespaceMappers.mapPathToNamespace(localPath);
            if (namespacePath != null) {
                var namespace = Dolittle.namespace(namespacePath);
                return namespace;
            }
            return null;
        }

        function getTypeNameFrom(path) {
            var localPath = Dolittle.Path.getPathWithoutFilename(path);
            var filename = Dolittle.Path.getFilenameWithoutExtension(path);
            return filename;
        }


        this.isLoaded = function (path) {
            var namespace = getNamespaceFrom(path);
            if (namespace != null) {
                var typename = getTypeNameFrom(path);
                return typename in namespace;
            }
            return false;
        };

        function getViewModelTypeForPathImplementation(path) {
            var namespace = getNamespaceFrom(path);
            if (namespace != null) {
                var typename = getTypeNameFrom(path);
                if (Dolittle.isType(namespace[typename])) {
                    return namespace[typename];
                }
            }

            return null;
        }

        this.getViewModelTypeForPath = function (path) {
            var type = getViewModelTypeForPathImplementation(path);
            if (Dolittle.isNullOrUndefined(type)) {
                var deepPath = path.replace(".js", "/index.js");
                type = getViewModelTypeForPathImplementation(deepPath);
                if (Dolittle.isNullOrUndefined(type)) {
                    deepPath = path.replace(".js", "/Index.js");
                    getViewModelTypeForPathImplementation(deepPath);
                }
            }

            return type;
        };


        this.beginCreateInstanceOfViewModel = function (path, region, viewModelParameters) {
            var promise = Dolittle.execution.Promise.create();

            var type = self.getViewModelTypeForPath(path);
            if (type != null) {
                var previousRegion = Dolittle.views.Region.current;
                Dolittle.views.Region.current = region;

                viewModelParameters = viewModelParameters || {};
                viewModelParameters.region = region;

                type.beginCreate(viewModelParameters)
                        .continueWith(function (instance) {
                            promise.signal(instance);
                        }).onFail(function (error) {
                            console.log("ViewModel '" + path + "' failed instantiation");
                            throw error;
                        });
            } else {
                console.log("ViewModel '" + path + "' does not exist");
                promise.signal(null);
            }

            return promise;
        };

    })
});
Dolittle.WellKnownTypesDependencyResolver.types.viewModelTypes = Dolittle.views.viewModelTypes;