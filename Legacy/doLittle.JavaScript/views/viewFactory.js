Dolittle.namespace("Dolittle.views", {
    viewFactory: Dolittle.Singleton(function () {
        this.createFrom = function (path) {
            var view = Dolittle.views.View.create({
                path: path
            });
            return view;
        };
    })
});
Dolittle.WellKnownTypesDependencyResolver.types.viewFactory = Dolittle.views.viewFactory;