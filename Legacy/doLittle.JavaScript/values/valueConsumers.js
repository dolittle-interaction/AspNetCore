Dolittle.namespace("Dolittle.values", {
    valueConsumers: Dolittle.Singleton(function () {

        this.getFor = function (instance, propertyName) {
            var consumer = Dolittle.values.DefaultValueConsumer.create({
                target: instance,
                property: propertyName
            });
            return consumer;
        };

    })
});
Dolittle.WellKnownTypesDependencyResolver.types.valueConsumers = Dolittle.values.valueConsumers;