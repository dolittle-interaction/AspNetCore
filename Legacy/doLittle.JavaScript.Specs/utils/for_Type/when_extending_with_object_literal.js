describe("when extending with object literal", function() {
	var exception;
	
	try {
		Dolittle.Type.extend({});		
	} catch(e) {
		exception = e;
	}
	
	it("should throw object literal not allowed exception", function() {
		expect(exception instanceof Dolittle.ObjectLiteralNotAllowed).toBeTruthy();
	});
});
