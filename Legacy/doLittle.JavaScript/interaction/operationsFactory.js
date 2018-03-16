Dolittle.namespace("Dolittle.interaction", {
    operationsFactory: Dolittle.Singleton(function () {
        this.create = function () {
            var operations = Dolittle.interaction.Operations.create();
            return operations;
        };
    })
});
Dolittle.WellKnownTypesDependencyResolver.types.operationsFactory = Dolittle.interaction.operationsFactory;