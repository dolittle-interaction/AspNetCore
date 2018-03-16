describe("when creating promise", function() {
	var promise = Dolittle.execution.Promise.create();

	it("should return an instance", function() {
		expect(promise).not.toBeNull();
	});
});