describe("when creating", function() {
	var instance = Dolittle.Import.create();
	it("should return an instance", function() {
		expect(instance).not.toBeUndefined();
	});
});