describe("when beginning to resolve", function () {
    var ns = {};
    var result;

    var configure = null;
    var dependencyResolvers;

    beforeEach(function () {
        configure = Dolittle.configure;
        Dolittle.configure = {
            ready: sinon.stub()
        };

        dependencyResolvers = Dolittle.dependencyResolvers;

        Dolittle.dependencyResolvers = {
            getAll: function () {
                return [{
                    canResolve: function () { return true; },
                    resolve: function () {

                        var promise = Dolittle.execution.Promise.create();
                        return promise;
                    }
                }];
            }
        };
        result = Dolittle.dependencyResolver.beginResolve(ns, "something");
    });

    afterEach(function () {
        Dolittle.dependencyResolvers = dependencyResolvers;
        Dolittle.configure = configure;
    });


	it("should return a promise", function() {
		expect(result instanceof Dolittle.execution.Promise).toBe(true);
	});
});