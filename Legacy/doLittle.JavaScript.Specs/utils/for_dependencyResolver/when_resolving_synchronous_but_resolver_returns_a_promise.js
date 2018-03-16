describe("when resolving synchronous but resolver returns a promise", function() {
	var exception;
	var ns = {

	}

	var dependencyResolvers;

	beforeEach(function () {
	    dependencyResolvers = Dolittle.dependencyResolvers;
	    Dolittle.dependencyResolvers = {
	        getAll: function () {
	            return [{
	                canResolve: function () {
	                    return true;
	                },
	                resolve: function () {
	                    return Dolittle.execution.Promise.create();
	                }
	            }
	            ];
	        }
	    };

	    try {
	        Dolittle.dependencyResolver.resolve(ns, "something");
	    } catch (e) {
	        exception = e;
	    }
	});

	afterEach(function () {
	    Dolittle.dependencyResolvers = dependencyResolvers;
	});
    

	it("should throw an exception", function() {
		expect(exception instanceof Dolittle.AsynchronousDependenciesDetected).toBe(true);
	});
});