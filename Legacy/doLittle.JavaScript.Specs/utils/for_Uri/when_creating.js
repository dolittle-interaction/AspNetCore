describe("when creating", function() {
	var firstInstance = Dolittle.Uri.create("http://www.vg.no");
	var secondInstance = Dolittle.Uri.create("http://www.vg.no");
	
	it("should return an instance", function() {
		expect(firstInstance).not.toBeUndefined();
	});
	
	it("should return unique instances", function() {
		expect(firstInstance).not.toBe(secondInstance);
	});
});