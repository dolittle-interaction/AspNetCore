Dolittle.namespace("Dolittle.values", {
    valueProviders: Dolittle.Singleton(function () {

        this.isKnown = function (name) {
            var found = false;
            var valueProviders = Dolittle.values.ValueProvider.getExtenders();
            valueProviders.forEach(function (valueProviderType) {
                if (valueProviderType._name.toLowerCase() === name) {
                    found = true;
                    return;
                }
            });
            return found;
        };

        this.getInstanceOf = function (name) {
            var instance = null;
            var valueProviders = Dolittle.values.ValueProvider.getExtenders();
            valueProviders.forEach(function (valueProviderType) {
                if (valueProviderType._name.toLowerCase() === name) {
                    instance = valueProviderType.create();
                    return;
                }
            });

            return instance;
        };
    })
});
Dolittle.WellKnownTypesDependencyResolver.types.valueProviders = Dolittle.values.valueProviders;