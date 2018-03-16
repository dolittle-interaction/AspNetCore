describe("when resolving asynchronous and resolver returns a promise", function() {
	var ns = {};
	var innerPromise = Dolittle.execution.Promise.create();
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
	                    return innerPromise;
	                }
	            }];
	        }
	    };

	    
	    Dolittle.dependencyResolver
            .beginResolve(ns, "something")
            .continueWith(function (arg, nextPromise) {
                result = arg;

            });
	    innerPromise.signal("Hello");

	    readyCallback();
	});

	afterEach(function () {
	    Dolittle.dependencyResolvers = dependencyResolvers;
	    Dolittle.configure = configure;
	});


	it("should continue with inner promise parameter when inner promise continues", function() {
		expect(result).toBe("Hello");
	});
});