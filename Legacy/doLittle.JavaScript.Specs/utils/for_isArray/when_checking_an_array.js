describe("when checking an array", function() {
	var result = Dolittle.isArray([]);
	it("should return true", function() {
		expect(result).toBe(true);
	});
});