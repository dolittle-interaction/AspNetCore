Dolittle.namespace("Dolittle.markup", {
    namespaceDefinitions: Dolittle.Singleton(function () {

        this.create = function (prefix) {
            var definition = Dolittle.markup.NamespaceDefinition.create({
                prefix: prefix,
            });
            return definition;
        };
    })
});