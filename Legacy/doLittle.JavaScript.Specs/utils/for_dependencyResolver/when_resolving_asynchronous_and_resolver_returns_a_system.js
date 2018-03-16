describe("when resolving asynchronous and resolver returns a system", function() {
    var ns = {};
    var result = null;

    var readyCallback;
    var configure = null;

    var dependencyResolvers;

    beforeEach(function () {
        configure = Dolittle.configure;
        Dolittle.configure = {
            ready: function (callback) {
                readyCallback = callback;
            }
        };

        dependencyResolvers = Dolittle.dependencyResolvers;
        Dolittle.dependencyResolvers = {
            getAll: function () {
                return [{
                    canResolve: function () {
                        return true;
                    },
                    resolve: function () {
                        return "The result";
                    }
                }];
            }
        };
        
        Dolittle.dependencyResolver
            .beginResolve(ns, "something")
            .continueWith(function (parameter, nextPromise) {
                result = parameter;
            });

        readyCallback();
    });

    afterEach(function () {
        Dolittle.dependencyResolvers = dependencyResolvers;
        Dolittle.configure = configure;
    });

	it("should continue with system from resolver as parameter", function() {
		expect(result).toBe("The result");
	});
});