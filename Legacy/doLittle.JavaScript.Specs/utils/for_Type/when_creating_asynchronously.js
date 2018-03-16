describe("when creating asynchronously", function() {
	var type = Dolittle.Type.extend(function() {
	});

	var result = type.beginCreate();

	it("should return a promise", function() {
		expect(result instanceof Dolittle.execution.Promise).toBe(true);
	});
});