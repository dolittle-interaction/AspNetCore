Dolittle.namespace("Dolittle.read", {
    queryableFactory: Dolittle.Singleton(function () {
        this.create = function (query, region) {
            var queryable = Dolittle.read.Queryable.new({
                query: query
            }, region);
            return queryable;
        };
    })
});
Dolittle.WellKnownTypesDependencyResolver.types.queryableFactory = Dolittle.interaction.queryableFactory;