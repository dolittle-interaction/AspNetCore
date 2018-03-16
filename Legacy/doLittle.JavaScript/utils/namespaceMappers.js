Dolittle.namespace("Dolittle",{
    namespaceMappers: {

        mapPathToNamespace: function (path) {
            for (var mapperKey in Dolittle.namespaceMappers) {
                var mapper = Dolittle.namespaceMappers[mapperKey];
                if (typeof mapper.hasMappingFor === "function" && mapper.hasMappingFor(path)) {
                    var namespacePath = mapper.resolve(path);
                    return namespacePath;
                }
            }

            return null;
        }
    }
});