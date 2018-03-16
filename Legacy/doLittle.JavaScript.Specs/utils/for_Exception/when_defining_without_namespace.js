describe("when defining without namespace", function() {
	Dolittle.Exception.define("SomeException");
	
	it("should put exception in global", function() {
		expect(SomeException).toBeDefined();
	});
	
	it("should define exception as function", function() {
		expect(typeof SomeException).toBe("function");
	});
});