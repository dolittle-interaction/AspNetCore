describe("when resolving and system is type", function() {
	var type = Dolittle.Type.extend(function(dependency) {
		this.something = "Hello";
		this.dependency = dependency;
	});

	var result = null;
	var dependencyResolvers;

	beforeEach(function () {
	    dependencyResolvers = Dolittle.dependencyResolvers;

	    Dolittle.dependencyResolvers = {
	        getAll: function () {
	            return [{
	                canResolve: function () {
	                    return true;
	                },
	                resolve: function (namespace, name) {
	                    if (name == "dependency") {
	                        return "dependency";
	                    }
	                    return type;
	                }
	            }];
	        }
	    };

	    var ns = {};
	    result = Dolittle.dependencyResolver.resolve(ns, "something");
	});

	afterEach(function () {
	    Dolittle.dependencyResolvers = dependencyResolvers;
	});
    

	it("should create instance of type and resolve dependencies", function() {
		expect(result.dependency).toBe("dependency");

	});
});