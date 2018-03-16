describe("when checking an object", function() {
	var result = Dolittle.isArray({});
	it("should return true", function() {
		expect(result).toBe(false);
	});
});