Dolittle.namespace("Dolittle",{
    WellKnownTypesDependencyResolver: function () {
        var self = this;
        this.types = Dolittle.WellKnownTypesDependencyResolver.types;

        this.canResolve = function (namespace, name) {
            return self.types.hasOwnProperty(name);
        };

        this.resolve = function (namespace, name) {
            return self.types[name];
        };
    }
});

Dolittle.WellKnownTypesDependencyResolver.types = {
    options: {}
};
