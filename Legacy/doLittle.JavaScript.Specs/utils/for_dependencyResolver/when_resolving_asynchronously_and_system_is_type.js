describe("when resolving asynchronously and system is type", function () {
    var type = Dolittle.Type.extend(function (dependency) {
        this.something = "Hello";
        this.dependency = dependency;
    });
    var ns = {
        something: type
    };
    var readyCallback;
    var configure = null;
    var result = null;

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
                    canResolve: function (namespace, name) {
                        return true;
                    },
                    resolve: function (namespace, name) {
                        var promise = Dolittle.execution.Promise.create();
                        var system = type;
                        if (name === "dependency") {
                            system = name;
                        }
                        promise.signal(system);
                        return promise;
                    }
                }];
            }
        };
        
        Dolittle.dependencyResolver
				.beginResolve(ns, "something")
				.continueWith(function (param, next) {
				    result = param;
				});

        readyCallback();
    });

    afterEach(function () {
        Dolittle.dependencyResolvers = dependencyResolvers;
        Dolittle.configure = configure;
    });


    // TODO: Fix this, or actually kill it off.. 
    /*
    it("should create instance of type and resolve dependencies", function () {
        expect(result.dependency).toBe("dependency");
    });*/
});