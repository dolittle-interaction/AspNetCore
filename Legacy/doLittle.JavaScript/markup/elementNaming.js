Dolittle.namespace("Dolittle.markup", {
    elementNaming: Dolittle.Singleton(function () {

        function getNameAndNamespace(element) {
            var namespace;
            var name = element.localName.toLowerCase();

            var namespaceSplit = name.split(":");
            if (namespaceSplit.length > 2) {
                throw Dolittle.markup.MultipleNamespacesInNameNotAllowed.create({ tagName: name });
            }
            if (namespaceSplit.length === 2) {
                name = namespaceSplit[1];
                namespace = namespaceSplit[0];
            }

            return {
                name: name,
                namespace: namespace
            };
        }


        this.getNamespacePrefixFor = function (element) {
            var nameAndNamespace = getNameAndNamespace(element);
            if (Dolittle.isNullOrUndefined(nameAndNamespace.namespace)) {
                return "";
            }
            return nameAndNamespace.namespace;
        };

        this.getLocalNameFor = function (element) {
            var nameAndNamespace = getNameAndNamespace(element);
            return nameAndNamespace.name;
        };
    })
});