describe("when extending with null parameter", function() {
	var exception;
	
	try {
		Dolittle.Type.extend(null);
	} catch(e) {
		exception = e;
	}
	
	it("should throw missing class definition exception", function() {
		expect(exception instanceof Dolittle.MissingTypeDefinition).toBeTruthy();
	});
});