describe("when defining without name", function() {
	var exception;
	
	try {
		Dolittle.Exception.define();
	} catch( e ) {
		exception = e;
	}
	
	it("should throw missing name exception", function() {
		expect(exception instanceof Dolittle.MissingName).toBeTruthy();
	});
});