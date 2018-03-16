describe("when resolving and system has multiple levels of inheritance but top level is a type", function () {

    var topLevelType = Dolittle.Type.extend(function() {
    });

    var secondLevelType = topLevelType.extend(function() {
    });

    var dependencyType = secondLevelType.extend(function () {
        this.hello = "world";
    });

	var type = Dolittle.Type.extend(function(dependency) {
		this.something = "Hello";
		this.dependency = dependency;
	});

	var result = null;
	var ns = {};

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
	                        return dependencyType;
	                    }
	                    return type;
	                }
	            }];
	        }
	    };

	    result = Dolittle.dependencyResolver.resolve(ns, "something");
	});

	afterEach(function () {
	    Dolittle.dependencyResolvers = dependencyResolvers;
	});
	

	it("should create instance of type and resolve dependencies", function() {
	    expect(result.dependency instanceof dependencyType).toBe(true);
	});
});