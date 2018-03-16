describe("when getting and properties starting with underscore exist on resolvers", function() {

	Dolittle.dependencyResolvers._something = { something: "hello"};

	var all = Dolittle.dependencyResolvers.getAll();

	it("should ignore the properties starting with underscore", function() {
		var hasUnderscore = false;
		for( var i=0; i<all.length; i++ ) {
			if( all[i].something === "hello" ) {
				hasUnderscore = true;
			}
		}

		expect(hasUnderscore).toBe(false);

	});
});