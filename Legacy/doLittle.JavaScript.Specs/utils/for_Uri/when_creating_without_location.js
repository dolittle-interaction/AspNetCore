describe("when creating without location", function() {
	var exception;
	try {
		Dolittle.Uri.create();
	} catch( e ) {
		exception = e;
	}
	
	it("should throw location not specified", function() {
		expect(exception instanceof Dolittle.LocationNotSpecified).toBeTruthy();
	});
});