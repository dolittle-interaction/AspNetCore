Dolittle.dependencyResolvers.DOMRootDependencyResolver = {
    canResolve: function (namespace, name) {
        return name === "DOMRoot";
    },

    resolve: function (namespace, name) {
        if (document.body != null && typeof document.body !== "undefined") {
            return document.body;
        }

        var promise = Dolittle.execution.Promise.create();
        Dolittle.dependencyResolvers.DOMRootDependencyResolver.promises.push(promise);
        return promise;
    }
};

Dolittle.dependencyResolvers.DOMRootDependencyResolver.promises = [];
Dolittle.dependencyResolvers.DOMRootDependencyResolver.documentIsReady = function () {
    Dolittle.dependencyResolvers.DOMRootDependencyResolver.promises.forEach(function (promise) {
        promise.signal(document.body);
    });
};