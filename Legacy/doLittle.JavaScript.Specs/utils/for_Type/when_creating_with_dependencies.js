describe("when creating with dependencies", function() {
	
	var somethingDependency;
	var system = {
		blah : "something"
	};

	var type = null; 

	var instance = null;

	beforeEach(function() {
		Dolittle.dependencyResolver = {
			getDependenciesFor: function() {
				return ["something"];
			},
			resolve : function(name) {
				return system;
			}
		};

		type = Dolittle.Type.extend(function(something) {
			somethingDependency = something;
		});		

		instance = type.create();
	});

	afterEach(function() {
		Dolittle.functionParser = {};
	});

	it("should create with resolved dependencies", function() {
		expect(somethingDependency).toBe(system);
	});
});